using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.CustomControls;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.Views
{
    public partial class MainView : UserControl
    {
        private LoadingForm _loadingForm;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;
        private readonly List<ItemControl> itemsControls;
        private readonly Order orders;
        private IEnumerable<Item> allItems;
        private string _title;

        private List<ItemControl> displayedItems;
        private const int ITEMS_PER_PAGE = 2;
        private int currentPage = 1;
        private int totalPages;

        private DateTime loadingStartTime;

        private decimal Total { get; set; } = default;

        public MainView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _employee = employee;
            itemsControls = new();
            orders = new();
            displayedItems = new();

            InitializeComponent();
            InitializeTimer();
            ShowLoadingForm();
        }

        private async void MainView_Load(object sender, EventArgs e)
        {
            await DisplayCategories();
            await ApplyTheme();
            await DisplayAppInfo();
            await DisplayItems();
        }

        public void ShowLoadingForm()
        {
            if (_loadingForm != null && !_loadingForm.IsDisposed)
            {
                return;
            }

            panelCover.Dock = DockStyle.Fill;
            panelRight.Visible = false;
            loadingStartTime = DateTime.Now;
            _loadingForm = new LoadingForm();
            _loadingForm.Show();
            _loadingForm.Refresh();

            Application.Idle += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= OnLoaded;
            _loadingForm.Close();

            panelCover.Dock = DockStyle.None;
            panelRight.Visible = true;
            panelCover.Visible = false;

            TimeSpan elapsedTime = DateTime.Now - loadingStartTime;
            MessageBox.Show("Time elapsed: " + elapsedTime.ToString());
        }


        private void ItemControl_AddToCartClicked(object sender, CartItemEventArgs e)
        {
            var existingCartItem = panelCart.Controls
                .OfType<CartControl>()
                .FirstOrDefault(cart => cart.CartItem.Item.Id == e.CartItem.Item.Id);

            var item = allItems.FirstOrDefault(i => i.Id == e.CartItem.Item.Id);

            if (existingCartItem != null)
            {
                if (e.CartItem.Quantity + existingCartItem.CartItem.Quantity > item.Stock)
                {
                    MessageDialog.Show(ParentForm, $"{item.Name} - Not enought stocks.",
                        "Insufficient Stocks", MessageDialogButtons.OK, MessageDialogIcon.Error,
                        MessageDialogStyle.Light);

                    return;
                }

                existingCartItem.CartItem.Quantity = (sender is ItemControl)
                    ? existingCartItem.CartItem.Quantity + e.CartItem.Quantity
                    : e.CartItem.Quantity;
                existingCartItem.InitializeCartItem();
            }
            else
            {
                var cartItem = new CartControl(e.CartItem, _styleManager);

                cartItem.RemoveFromCartClicked += CartControl_RemoveFromCartClicked!;
                cartItem.AddToCartClicked += ItemControl_AddToCartClicked!;

                panelCart.Controls.Add(cartItem);
                orders.Items.Add(e.CartItem);
            }

            UpdateTotalValue();
            UpdateTotalDisplay();
        }

        private void CategoryControl_CategoryClicked(object sender, CategoryEventArgs e)
        {
            var filteredItemControls = itemsControls.Where(i => i.Item.CategoryId == e.Category.Id)
                .ToList();

            DisplayFilteredItems(filteredItemControls);

            btnAllCategory.FillColor = Color.FromArgb(248, 248, 248);
            btnAllCategory.ForeColor = Color.FromArgb(48, 48, 48);

            var category = sender as CategoryControl;

            flowLeft.Controls.OfType<CategoryControl>()
                .Where(c => c != category)
                .ToList()
                .ForEach(c => c.ChangeToDefault());
        }

        private void CartControl_RemoveFromCartClicked(object sender, CartItemEventArgs e)
        {
            var cartControl = sender as CartControl;
            panelCart.Controls.Remove(cartControl);
            orders.Items.Remove(e.CartItem);

            UpdateTotalValue();
            UpdateTotalDisplay();
        }

        private async Task DisplayItems()
        {
            allItems = await _unitOfWork.ItemRepo
                .Get(includeProperties: "Category");

            itemsControls.AddRange(allItems
                .Select(item => new ItemControl(item, _styleManager))
                .ToList());

            totalPages = (int)Math.Ceiling((double)itemsControls.Count / ITEMS_PER_PAGE);
            DisplayCurrentPage();
        }

        private void DisplayCurrentPage()
        {
            panelItems.Controls.Clear();
            displayedItems = itemsControls.Skip((currentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE)
                .ToList();

            foreach (var control in displayedItems)
            {
                control.AddToCartClicked += ItemControl_AddToCartClicked!;
            }

            panelItems.Controls.AddRange(displayedItems.ToArray());

            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            panelPage.Controls.Clear();

            if (currentPage > 1)
            {
                AddPageButton("Previous", currentPage - 1);
            }

            AddPageButton("1", 1);

            var startPage = Math.Max(2, currentPage - 2);
            var endPage = Math.Min(startPage + 3, totalPages);

            if (startPage > 2)
            {
                AddEllipsisButton();
            }

            for (int i = startPage; i <= endPage; i++)
            {
                AddPageButton($"{i}", i);
            }

            if (endPage < totalPages)
            {
                AddEllipsisButton();
            }

            if (totalPages > 1)
            {
                AddPageButton($"Last", totalPages);
            }

            if (currentPage < totalPages)
            {
                AddPageButton("Next", currentPage + 1);
            }
        }

        private void AddPageButton(string text, int pageNum)
        {
            var pageButton = new Button();
            pageButton.Text = text;

            if (pageNum == currentPage)
            {
                pageButton.BackColor = Color.Blue;
                pageButton.ForeColor = Color.White;
            }

            pageButton.Click += (sender, e) =>
            {
                currentPage = pageNum;
                DisplayCurrentPage();
            };
            panelPage.Controls.Add(pageButton);
        }

        private void AddEllipsisButton()
        {
            var ellipsisButton = new Button();
            ellipsisButton.Text = "...";
            ellipsisButton.Enabled = false;
            panelPage.Controls.Add(ellipsisButton);
        }

        private async Task RefreshItems()
        {
            itemsControls.Clear();
            panelItems.Controls.Clear();
            await DisplayItems();
            await DisplayTransactionId();
        }

        private void DisplayFilteredItems(IEnumerable<ItemControl> filteredItems = null)
        {
            var itemsToDisplay = filteredItems ?? itemsControls;
            panelItems.Controls.Clear();

            foreach (var item in itemsToDisplay)
            {
                panelItems.Controls.Add(item);
            }
        }

        private async Task DisplayCategories()
        {
            var categories = await _unitOfWork.CategoryRepo.Get();
            var categoryControls = categories.Select(c =>
            {
                var control = new CategoryControl(c, _styleManager);
                control.CategoryClicked += CategoryControl_CategoryClicked!;
                return control;
            }).ToList();

            flowLeft.Controls.AddRange(categoryControls.ToArray());
        }

        private async Task DisplayAppInfo()
        {
            var appDataList = await _unitOfWork.AppSettingsRepo.Get();
            var appSettings = appDataList.FirstOrDefault();
            _title = appSettings.Name;
            lblTitle.Text = _title;
            Text = _title;
            lblTime.Text = $"Date: {DateTime.Now:dddd, hh:mmtt MM/dd/yy}";
            await DisplayTransactionId();
        }

        private void UpdateTotalValue()
        {
            Total = orders.Items.Sum(order => order.SubTotal);
        }

        private void UpdateTotalDisplay()
        {
            lblTotal.Text = $"₱ {Total:#,###.00}";
        }

        private void ClearCart()
        {
            orders.Items.Clear();
            Total = default;
            UpdateTotalDisplay();
            panelCart.Controls.Clear();
        }

        private async void btnPayNow_Click(object sender, EventArgs e)
        {
            if (orders.Items.Count <= 0)
            {
                MessageDialog.Show(ParentForm, "Please enter order first", "Information",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            if (!await ValidateStocks())
            {
                MessageDialog.Show(ParentForm, "Item quantity cannot exceed item stocks. Please check your order again.",
                    "Payment Failed", MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }
            var paymentForm = new PaymentForm(orders, Total, _unitOfWork, _employee, _styleManager);
            paymentForm.ShowDialog();
            await RefreshItems();
            ClearCart();
        }

        private async Task<bool> ValidateStocks()
        {
            foreach (var order in orders.Items)
            {
                var item = await _unitOfWork.ItemRepo.GetByID(order.Item.Id);
                if (item == null || order.Quantity > item.Stock)
                {
                    return false;
                }
            }
            return true;
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToButton(btnPayNow);
            await _styleManager.Theme.ApplyOutlineThemeToButton(btnSearch);
        }

        private async void btnPayLater_Click(object sender, EventArgs e)
        {
            if (orders.Items.Count <= 0)
            {
                MessageDialog.Show(ParentForm, "Please enter order first", "Information",
                    MessageDialogButtons.OK, MessageDialogIcon.Warning, MessageDialogStyle.Light);
                return;
            }

            try
            {
                var transaction = await CreateTransaction();
                _unitOfWork.TransactionRepo.Insert(transaction);
                await _unitOfWork.SaveAsync();

                MessageDialog.Show(ParentForm, "Order added to pay later", "Order Added",
                    MessageDialogButtons.OK, MessageDialogIcon.Information, MessageDialogStyle.Light);
                await RefreshItems();
                ClearCart();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ParentForm, "An error occurred: " + ex.Message, "Order Failed",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
            }
        }

        private async Task<Transaction> CreateTransaction()
        {
            var transaction = new Transaction
            {
                EmployeeId = _employee.Id,
                TransactionDate = DateTime.Now,
                TotalAmount = Total,
                AmountPaid = default,
                Change = default,
                IsCompleted = false,
                Items = new List<TransactionItem>()
            };

            await SubtractStocks(transaction.Items);
            return transaction;
        }

        private async Task SubtractStocks(ICollection<TransactionItem> transaction)
        {
            foreach (var order in orders.Items)
            {
                var item = await _unitOfWork.ItemRepo.GetByID(order.Item.Id);

                if (item != null)
                {
                    var soldQuantity = order.Quantity;
                    var availableQuantity = item.Stock;

                    if (soldQuantity <= availableQuantity)
                    {
                        var transactionItem = new TransactionItem
                        {
                            ItemId = order.Item.Id,
                            Quantity = soldQuantity,
                            SubTotal = order.SubTotal
                        };

                        transaction.Add(transactionItem);
                        item.Stock -= soldQuantity;
                    }
                    else
                    {
                        throw new Exception("Item quantity cannot exceed item stocks. Please check your order again.");
                    }
                }
                else
                {
                    MessageDialog.Show(ParentForm, "Transaction failed.", "Error Occured",
                        MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                }
            }
        }

        private void btnAllCategory_Click(object sender, EventArgs e)
        {
            //itemsControls.Clear();
            panelItems.Controls.Clear();
            DisplayFilteredItems(itemsControls);

            btnAllCategory.FillColor = Color.FromArgb(48, 48, 48);
            btnAllCategory.ForeColor = Color.FromArgb(248, 248, 248);

            flowLeft.Controls.OfType<CategoryControl>()
                .ToList()
                .ForEach(c => c.ChangeToDefault());
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            var form = new ProfileForm(_unitOfWork, _styleManager, _employee);
            form.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearCart();
        }

        private async Task DisplayTransactionId()
        {
            var transactionId = await _unitOfWork.TransactionRepo.GetLatestTransactionId();
            lblTransacId.Text = $"Transaction #: {transactionId}";
        }

        private void InitializeTimer()
        {
            timerDate.Tick += timerDate_Tick!;
            timerDate.Start();
        }

        private void timerDate_Tick(object sender, EventArgs e)
        {
            lblTime.Text = $"Date: {DateTime.Now:dddd, hh:mmtt MM/dd/yy}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            panelItems.Controls.Clear();

            var query = txtSearch.Text;
            var filteredItems = itemsControls
                .Where(item => item.Item.Name
                    .Contains(query, StringComparison.OrdinalIgnoreCase));

            foreach (var item in filteredItems)
            {
                panelItems.Controls.Add(item);
            }
        }
    }
}