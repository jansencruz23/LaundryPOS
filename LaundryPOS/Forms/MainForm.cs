using LaundryPOS.Contracts;
using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.Views;
using LaundryPOS.Models;
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
        private readonly List<ItemControl> items;
        private Order order = new();

        private double Total { get; set; } = default;

        public MainForm(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            items = new();
            InitializeComponent();
            DisplayServices();
        }

        private void ItemControl_AddToCartClicked(object sender, CartItemEventArgs e)
        {
            var existingCartItem = cartPanel.Controls
                .OfType<CartControl>()
                .FirstOrDefault(cart => cart.CartItem.Item.ItemId == e.CartItem.Item.ItemId);

            if (existingCartItem != null)
            {
                existingCartItem.CartItem.Quantity += e.CartItem.Quantity;
                existingCartItem.InitializeCartItem();
            }
            else
            {
                var cartItem = new CartControl(e.CartItem);
                cartItem.RemoveFromCartClicked += CartControl_RemoveFromCartClicked!;
                cartPanel.Controls.Add(cartItem);
            }

            Total += e.CartItem.SubTotal;
            UpdateTotalDisplay();
        }

        private void CartControl_RemoveFromCartClicked(object sender, CartItemEventArgs e)
        {
            cartPanel.Controls.Remove(sender as CartControl);
            Total -= e.CartItem.SubTotal;
            UpdateTotalDisplay();
        }

        private async void DisplayServices()
        {
            var services = await _unitOfWork.ServiceRepo.Get();

            items.AddRange(services
                .Select(service => new ItemControl(service)));

            items.ForEach(item =>
            {
                item.AddToCartClicked += ItemControl_AddToCartClicked!;
                itemsPanel.Controls.Add(item);
            });
        }

        private void UpdateTotalDisplay()
        {
            lblTotal.Text = $"{Total:#,###.00}";
        }
    }
}