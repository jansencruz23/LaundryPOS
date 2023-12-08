using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.CustomControls;
using LaundryPOS.Helpers;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.Views
{
    public partial class MainView : UserControl
    {
        private LoadingForm _loadingForm;
        private PaginationHelper<ItemControl> _paginationHelper;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;
        private readonly List<ItemControl> itemsControls;
        private IEnumerable<ItemControl> _filteredItems;
        private readonly Order orders;
        private IEnumerable<Item> allItems;
        private string _title;

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

            InitializeComponent();
            InitializeTimer();
            ShowLoadingForm();

            _paginationHelper = new(panelPage, _styleManager);
            _paginationHelper.PageClicked += PageClicked;
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
            _paginationHelper.CurrentPage = 1;
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

        private void PageClicked(object? sender, EventArgs e)
        {
            DisplayCurrentPage(_filteredItems ?? itemsControls);
        }

        private async Task DisplayItems()
        {
            allItems = await _unitOfWork.ItemRepo.Get(includeProperties: "Category");

            itemsControls.Clear();
            itemsControls.AddRange(allItems
                .Select(item => new ItemControl(item, _styleManager))
                .ToList());

            DisplayCurrentPage();
        }

        private void DisplayCurrentPage(IEnumerable<ItemControl> filteredItems = null)
        {
            panelItems.Controls.Clear();

            _filteredItems = filteredItems ?? itemsControls;
            filteredItems = _paginationHelper.GetPagedItems(_filteredItems);

            foreach (var control in filteredItems)
            {
                control.AddToCartClicked -= ItemControl_AddToCartClicked!;
                control.AddToCartClicked += ItemControl_AddToCartClicked!;
            }

            panelItems.Controls.AddRange(filteredItems.ToArray());
        }

        private void DisplayFilteredItems(IEnumerable<ItemControl> filteredItems = null)
        {
            panelItems.Controls.Clear();
            DisplayCurrentPage(filteredItems);
        }

        private async Task RefreshItems()
        {
            itemsControls.Clear();
            panelItems.Controls.Clear();
            await DisplayItems();
            await DisplayTransactionId();
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
            panelItems.Controls.Clear();
            DisplayFilteredItems();

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
            panelPage.Controls.Clear();

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