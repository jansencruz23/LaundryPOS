using LaundryPOS.CustomEventArgs;
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

namespace LaundryPOS.Forms.Views
{
    public partial class ItemControl : UserControl
    {
        public event EventHandler<CartItemEventArgs> AddToCartClicked;
        private readonly Item _service;

        public ItemControl(Item service)
        {
            _service = service;

            InitializeComponent();
            InitializeService();
            WireAllControls(this);
        }

        private void InitializeService()
        {
            lblName.Text = _service.Name;
            lblPrice.Text = _service.Price.ToString();
            imgIcon.Image = Image.FromFile(!string.IsNullOrEmpty(_service.PicPath) ? _service.PicPath : "./default.png");
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
            var quantityForm = new QuantityForm();
            quantityForm.ShowDialog();
            var quantity = quantityForm.Quantity;
            AddToCartClicked?.Invoke(this, new CartItemEventArgs(_service, quantity));
        }
    }
}