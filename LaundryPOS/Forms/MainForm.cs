using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.CustomControls;
using LaundryPOS.Forms.Views;
using LaundryPOS.Models;
using LaundryPOS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms
{
    public partial class MainForm : Form
    {
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
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await DisplayItems();
            await DisplayCategories();
            await ApplyTheme();
            await DisplayAppInfo();
            DisplayEmployeeInfo();
        }

        private void ItemControl_AddToCartClicked(object sender, CartItemEventArgs e)
        {
            var existingCartItem = cartPanel.Controls
                .OfType<CartControl>()
                .FirstOrDefault(cart => cart.CartItem.Item.ItemId == e.CartItem.Item.ItemId);

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

                cartPanel.Controls.Add(cartItem);
                orders.Items.Add(e.CartItem);
            }

            UpdateTotalValue();
            UpdateTotalDisplay();
        }

        private void CategoryControl_CategoryClicked(object sender, CategoryEventArgs e)
        {
            var filteredItems = allItems.Where(i => i.CategoryId == e.Category.CategoryId);
            DisplayFilteredItems(filteredItems);
            lblMenuTitle.Text = e.Category.Name;
        }

        private void CartControl_RemoveFromCartClicked(object sender, CartItemEventArgs e)
        {
            var cartControl = sender as CartControl;
            cartPanel.Controls.Remove(cartControl);
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
                itemsPanel.Controls.Add(item);
            }
        }

        private async Task RefreshItems()
        {
            itemsControls.Clear();
            itemsPanel.Controls.Clear();
            await DisplayItems();
        }

        private void DisplayEmployeeInfo()
        {
            lblEmployeeName.Text = _employee.FullName;
            imgPic.Image = Image.FromFile(!string.IsNullOrWhiteSpace(_employee.PicPath)
                ? _employee.PicPath
                : "./default.png");
        }

        private void DisplayFilteredItems(IEnumerable<Item> filteredItems = null)
        {
            var itemsToDisplay = filteredItems ?? allItems;
            itemsControls.Clear();
            itemsPanel.Controls.Clear();

            itemsControls.AddRange(itemsToDisplay
                .Select(item => new ItemControl(item, _themeManager)));

            foreach (var item in itemsControls)
            {
                item.AddToCartClicked += ItemControl_AddToCartClicked!;
                itemsPanel.Controls.Add(item);
            }
        }

        private async Task DisplayCategories()
        {
            var categories = await _unitOfWork.CategoryRepo.Get();
            var categoryControls = categories.Select(c =>
            {
                var control = new CategoryControl(c);
                control.CategoryClicked += CategoryControl_CategoryClicked!;
                return control;
            }).ToList();

            categoryPanel.Controls.AddRange(categoryControls.ToArray());
        }

        private async Task DisplayAppInfo()
        {
            var appName = await _unitOfWork.AppSettingsRepo.GetByID(APP_SETTINGS_INDEX);
            lblTitle.Text = appName.Name;
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
            cartPanel.Controls.Clear();
        }

        private void btnPayNow_Click(object sender, EventArgs e)
        {
            if (orders.Items.Count <= 0)
            {
                MessageBox.Show("Please enter order first");
                return;
            }

            var paymentForm = new PaymentForm(orders, Total, _unitOfWork, _employee, _themeManager);
            paymentForm.ShowDialog();
            ClearCart();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnPayNow);
            await _themeManager.ApplyThemeToButton(btnViewUnpaid);
            await _themeManager.ApplyThemeToPanel(bgPanel);
        }

        private async void btnPayLater_Click(object sender, EventArgs e)
        {
            if (orders.Items.Count <= 0)
            {
                MessageBox.Show("Please enter order first");
                return;
            }

            try
            {
                var transaction = await CreateTransaction();
                _unitOfWork.TransactionRepo.Insert(transaction);
                await _unitOfWork.SaveAsync();

                MessageBox.Show("Order added to pay later");
                await RefreshItems();
                ClearCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private async Task<Transaction> CreateTransaction()
        {
            var transaction = new Transaction
            {
                EmployeeId = _employee.EmployeeId,
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
                var item = await _unitOfWork.ItemRepo.GetByID(order.Item.ItemId);

                if (item != null)
                {
                    var soldQuantity = order.Quantity;
                    var availableQuantity = item.Stock;

                    if (soldQuantity <= availableQuantity)
                    {
                        var transactionItem = new TransactionItem
                        {
                            ItemId = order.Item.ItemId,
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
                    MessageBox.Show("Transaction failed.");
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
            itemsPanel.Controls.Clear();
            await DisplayItems();
            lblMenuTitle.Text = "All Items";
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
    }
}