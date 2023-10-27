using LaundryPOS.CustomEventArgs;
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
    public partial class ItemControl : UserControl
    {
        public event EventHandler<CartItemEventArgs> AddToCartClicked;
        private readonly ThemeManager _themeManager;
        public Item Item { get; set; }

        public ItemControl(Item item,
            ThemeManager themeManager)
        {
            Item = item;
            _themeManager = themeManager;

            InitializeComponent();
            InitializeItem();
            WireAllControls(this);
        }

        private async void ItemControl_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            if (Item.Stock <= 0)
            {
                Enabled = false;
            }
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToLabel(lblPrice);
        }

        private void InitializeItem()
        {
            lblName.Text = Item.Name;
            lblCategory.Text = Item.Category.Name;
            lblPrice.Text = $"₱{Item.Price:#,###.00}";
            lblStock.Text = $"{Item.Stock} left";
            imgPic.Image = GetImage();
        }

        private Image GetImage()
        {
            try
            {
                return Item.Image != null
                ? Image.FromFile(Item.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }


        private void WireAllControls(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                ctrl.Click += Control_Click!;
                if (ctrl.HasChildren)
                {
                    WireAllControls(ctrl);
                }
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, EventArgs.Empty);

            var quantityForm = new QuantityForm(_themeManager);

            if (quantityForm.ShowDialog() == DialogResult.Cancel)
                return;

            var quantity = quantityForm.Quantity;

            if (quantity > Item.Stock)
            {
                MessageBox.Show("Stocks not enough");
                return;
            }

            AddToCartClicked?.Invoke(this, 
                new CartItemEventArgs(
                    new CartItem(Item, quantity)));
        }
    }
}