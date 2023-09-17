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
    public partial class CartControl : UserControl
    {
        public event EventHandler RemoveFromCartClicked;
        private readonly Item _service;
        private int _quantity;

        public double SubTotal { get => _service.Price * _quantity; }
        public Item Service { get => _service;  }
        public int Quantity { get => _quantity; set { _quantity = value; } }

        public CartControl(Item service, int quantity)
        {
            _service = service;
            _quantity = quantity;
            InitializeComponent();
            InitializeCartItem();
        }

        public void InitializeCartItem()
        {
            lblName.Text = _service.Name;
            lblPrice.Text = _service.Price.ToString();
            lblSubTotal.Text = SubTotal.ToString();
            btnQuantity.Text = _quantity.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveFromCartClicked?.Invoke(this, new CartItemEventArgs(_service, _quantity));
        }
    }
}