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
        private readonly Service _service;
        private int _quantity;

        public double SubTotal { get => _service.Price * _quantity; }

        public CartControl(Service service, int quantity)
        {
            _service = service;
            _quantity = quantity;
            InitializeComponent();
            InitializeCartItem();
        }

        private void InitializeCartItem()
        {
            lblName.Text = _service.Name;
            lblPrice.Text = _service.Price.ToString();
            lblSubTotal.Text = SubTotal.ToString();
            btnQuantity.Text = _quantity.ToString();
        }
    }
}