﻿using LaundryPOS.CustomEventArgs;
using LaundryPOS.Models;
using LaundryPOS.Contracts;
using Guna.UI2.WinForms;

namespace LaundryPOS.Forms.Views
{
    public partial class ItemControl : UserControl
    {
        public event EventHandler<CartItemEventArgs> AddToCartClicked;
        private readonly IStyleManager _styleManager;
        public Item Item { get; set; }

        public ItemControl(Item item,
            IStyleManager styleManager)
        {
            Item = item;
            _styleManager = styleManager;
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
            await _styleManager.Theme.ApplyThemeToLabel(lblPrice);
        }

        private void InitializeItem()
        {
            lblName.Text = Item.Name;
            lblCategory.Text = Item.Category != null 
                ? Item.Category.Name 
                : string.Empty;
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

            var quantityForm = new QuantityForm(_styleManager);

            if (quantityForm.ShowDialog() == DialogResult.Cancel)
                return;

            var quantity = quantityForm.Quantity;

            if (quantity > Item.Stock)
            {
                MessageDialog.Show(ParentForm, $"{Item.Name} - Not enought stocks.","Insufficient Stocks",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            AddToCartClicked?.Invoke(this, 
                new CartItemEventArgs(
                    new CartItem(Item, quantity)));
        }
    }
}