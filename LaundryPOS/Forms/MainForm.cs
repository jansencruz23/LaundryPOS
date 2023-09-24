using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
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
        private readonly List<ItemControl> items;
        private readonly Order orders;

        private decimal Total { get; set; } = default;

        public MainForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;
            items = new();
            orders = new();

            InitializeComponent();
            DisplayItems();
            ApplyTheme();
        }

        private void ItemControl_AddToCartClicked(object sender, CartItemEventArgs e)
        {
            var existingCartItem = cartPanel.Controls
                .OfType<CartControl>()
                .FirstOrDefault(cart => cart.CartItem.Item.ItemId == e.CartItem.Item.ItemId);

            if (existingCartItem != null)
            {
                existingCartItem.CartItem.Quantity = e.CartItem.Quantity;
                existingCartItem.InitializeCartItem();
            }
            else
            {
                var cartItem = new CartControl(e.CartItem);
                cartItem.RemoveFromCartClicked += CartControl_RemoveFromCartClicked!;
                cartPanel.Controls.Add(cartItem);
                orders.Items.Add(e.CartItem);
            }

            UpdateTotalValue();
            UpdateTotalDisplay();
        }

        private void CartControl_RemoveFromCartClicked(object sender, CartItemEventArgs e)
        {
            var cartControl = sender as CartControl;
            cartPanel.Controls.Remove(cartControl);
            orders.Items.Remove(e.CartItem);

            UpdateTotalValue();
            UpdateTotalDisplay();
        }

        private async void DisplayItems()
        {
            var items = await _unitOfWork.ItemRepo.Get();
            this.items.AddRange(items
                .Select(item => new ItemControl(item)));

            this.items.ForEach(item =>
            {
                item.AddToCartClicked += ItemControl_AddToCartClicked!;
                itemsPanel.Controls.Add(item);
            });
        }

        private void UpdateTotalValue()
        {
            Total = orders.Items.Sum(order => order.SubTotal);
        }

        private void UpdateTotalDisplay()
        {
            lblTotal.Text = $"{Total:#,###.00}";
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
            var paymentForm = new PaymentForm(orders, Total, _unitOfWork, _employee);
            paymentForm.ShowDialog();
            ClearCart();
        }

        private async void ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnPayNow);
        }

        private async void btnPayLater_Click(object sender, EventArgs e)
        {
            try
            {
                var transaction = CreateTransaction();
                _unitOfWork.TransactionRepo.Insert(transaction);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private Transaction CreateTransaction() => new()
        {
            EmployeeId = _employee.EmployeeId,
            TransactionDate = DateTime.Now,
            TotalAmount = Total,
            AmountPaid = default,
            Change = default,
            IsCompleted = false,
            Items = orders.Items.Select(order => new TransactionItem
            {
                ItemId = order.Item.ItemId,
                Quantity = order.Quantity,
                SubTotal = order.SubTotal
            }).ToList()
        };

        private void btnViewUnpaid_Click(object sender, EventArgs e)
        {
            Hide();
            var form = new UnpaidForm(_unitOfWork, _themeManager, _employee);
            form.FormClosed += (s, args) => Close();
            form.Show();
        }
    }
}