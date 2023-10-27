﻿using LaundryPOS.CustomEventArgs;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms.Views
{
    public partial class CartControl : UserControl
    {
        public event EventHandler<CartItemEventArgs> RemoveFromCartClicked;
        public event EventHandler<CartItemEventArgs> AddToCartClicked;
        public CartItem CartItem { get; }
        private ThemeManager _themeManager;

        public CartControl(CartItem cartItem,
            ThemeManager themeManager)
        {
            CartItem = cartItem;
            _themeManager = themeManager;

            InitializeComponent();
            InitializeCartItem();
        }

        private async void CartControl_Load(object sender, EventArgs e)
        {
            await _themeManager.ApplyOutlineThemeToButton(btnQuantity);
        }

        public void InitializeCartItem()
        {
            lblName.Text = CartItem.Item.Name;
            lblCategory.Text = CartItem.Item.Category.Name;
            btnQuantity.Text = CartItem.Quantity.ToString();
            lblSubTotal.Text = $"₱{CartItem.SubTotal:#,###.00}";
            imgPic.Image = GetImage();
        }
        private Image GetImage()
        {
            try
            {
                return CartItem.Item.Image != null
                ? Image.FromFile(CartItem.Item.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveFromCartClicked?.Invoke(this, new CartItemEventArgs(CartItem));
        }

        private void btnQuantity_Click(object sender, EventArgs e)
        {
            var quantityForm = new QuantityForm(_themeManager);
            quantityForm.Quantity = CartItem.Quantity;
            quantityForm.ShowDialog();
            var quantity = quantityForm.Quantity;

            AddToCartClicked?.Invoke(this,
                new CartItemEventArgs(
                    new CartItem(CartItem.Item, quantity)));
        }
    }
}