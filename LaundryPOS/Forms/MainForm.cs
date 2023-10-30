using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.CustomControls;
using LaundryPOS.Forms.Views;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using System.Data;

namespace LaundryPOS.Forms
{
    public partial class MainForm : Form
    {
        private LoadingForm _loadingForm;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly Employee _employee;
        private readonly List<ItemControl> itemsControls;
        private readonly Order orders;
        private IEnumerable<Item> allItems;
        private const int APP_SETTINGS_INDEX = 1;

        private decimal Total { get; set; } = default;

        public MainForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;
            itemsControls = new();
            orders = new();

            InitializeComponent();
            InitializeTimer();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            ShowLoadingForm();
            await DisplayCategories();
            await ApplyTheme();
            await DisplayAppInfo();
            DisplayEmployeeInfo();
            await DisplayItems();
        }

        private void ShowLoadingForm()
        {
            if (_loadingForm != null && !_loadingForm.IsDisposed)
            {
                return;
            }

            _loadingForm = new LoadingForm();
            _loadingForm.Show();
            _loadingForm.Refresh();

            Application.Idle += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= OnLoaded;
            _loadingForm.Close();
            Opacity = 100;
        }

        private void ItemControl_AddToCartClicked(object sender, CartItemEventArgs e)
        {
            var existingCartItem = panelCart.Controls
                .OfType<CartControl>()
                .FirstOrDefault(cart => cart.CartItem.Item.Id == e.CartItem.Item.Id);

            if (existingCartItem != null)
            {
                existingCartItem.CartItem.Quantity = (sender is ItemControl)
                    ? existingCartItem.CartItem.Quantity + e.CartItem.Quantity
                    : e.CartItem.Quantity;
                existingCartItem.InitializeCartItem();
            }
            else
            {
                var cartItem = new CartControl(e.CartItem, _themeManager);

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
            var filteredItems = allItems.Where(i => i.CategoryId == e.Category.Id);
            DisplayFilteredItems(filteredItems);

            btnAllCategory.FillColor = Color.FromArgb(248, 248, 248);
            btnAllCategory.ForeColor = Color.FromArgb(48,48,48);

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
            allItems = await _unitOfWork.ItemRepo.Get(includeProperties: "Category");
            itemsControls.AddRange(allItems
                .Select(item => new ItemControl(item, _themeManager)));

            foreach (var item in itemsControls)
            {
                item.AddToCartClicked += ItemControl_AddToCartClicked!;
                panelItems.Controls.Add(item);
            }
        }

        private async Task RefreshItems()
        {
            itemsControls.Clear();
            panelItems.Controls.Clear();
            await DisplayItems();
            await DisplayTransactionId();
        }

        private void DisplayEmployeeInfo()
        {
            imgPic.Image = Image.FromFile(!string.IsNullOrWhiteSpace(_employee.Image)
                ? _employee.Image
                : "./default.png");
        }

        private void DisplayFilteredItems(IEnumerable<Item> filteredItems = null)
        {
            var itemsToDisplay = filteredItems ?? allItems;
            itemsControls.Clear();
            panelItems.Controls.Clear();

            itemsControls.AddRange(itemsToDisplay
                .Select(item => new ItemControl(item, _themeManager)));

            foreach (var item in itemsControls)
            {
                item.AddToCartClicked += ItemControl_AddToCartClicked!;
                panelItems.Controls.Add(item);
            }
        }

        private async Task DisplayCategories()
        {
            var categories = await _unitOfWork.CategoryRepo.Get();
            var categoryControls = categories.Select(c =>
            {
                var control = new CategoryControl(c, _themeManager);
                control.CategoryClicked += CategoryControl_CategoryClicked!;
                return control;
            }).ToList();

            flowLeft.Controls.AddRange(categoryControls.ToArray());
        }

        private async Task DisplayAppInfo()
        {
            var appName = await _unitOfWork.AppSettingsRepo.GetByID(APP_SETTINGS_INDEX);
            lblTitle.Text = appName.Name;
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
                MessageBox.Show("Please enter order first", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!await ValidateStocks())
            {
                MessageBox.Show("Item quantity cannot exceed item stocks. Please check your order again.", 
                    "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var paymentForm = new PaymentForm(orders, Total, _unitOfWork, _employee, _themeManager);
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
            await _themeManager.ApplyThemeToButton(btnPayNow);
            await _themeManager.ApplyThemeToButton(btnViewUnpaid);
        }

        private async void btnPayLater_Click(object sender, EventArgs e)
        {
            if (orders.Items.Count <= 0)
            {
                MessageBox.Show("Please enter order first", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var transaction = await CreateTransaction();
                _unitOfWork.TransactionRepo.Insert(transaction);
                await _unitOfWork.SaveAsync();

                MessageBox.Show("Order added to pay later", "Order Added",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshItems();
                ClearCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Order Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Transaction failed.", "Error Occured",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewUnpaid_Click(object sender, EventArgs e)
        {
            Hide();
            var form = new UnpaidForm(_unitOfWork, _themeManager, _employee);
            form.FormClosed += (s, args) => Close();
            form.Show();
        }

        private async void btnAllCategory_Click(object sender, EventArgs e)
        {
            itemsControls.Clear();
            panelItems.Controls.Clear();
            await DisplayItems();

            btnAllCategory.FillColor = Color.FromArgb(48, 48, 48);
            btnAllCategory.ForeColor = Color.FromArgb(248, 248, 248);

            flowLeft.Controls.OfType<CategoryControl>()
                .ToList()
                .ForEach(c => c.ChangeToDefault());
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            var form = new ProfileForm(_unitOfWork, _themeManager, _employee);
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
    }
}