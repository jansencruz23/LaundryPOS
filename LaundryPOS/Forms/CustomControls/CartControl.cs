using LaundryPOS.CustomEventArgs;
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

namespace LaundryPOS.Forms.Views
{
    public partial class CartControl : UserControl
    {
        public event EventHandler<CartItemEventArgs> RemoveFromCartClicked;
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
            await _themeManager.ApplyThemeToButton(btnQuantity);
        }

        public void InitializeCartItem()
        {
            lblName.Text = CartItem.Item.Name;
            lblPrice.Text = $"₱ {CartItem.Item.Price:#,###.00}";
            btnQuantity.Text = CartItem.Quantity.ToString();
            lblSubTotal.Text = $"₱ {CartItem.SubTotal:#,###.00}";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveFromCartClicked?.Invoke(this, new CartItemEventArgs(CartItem));
        }
    }
}