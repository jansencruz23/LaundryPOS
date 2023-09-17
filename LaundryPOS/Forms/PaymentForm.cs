using LaundryPOS.Forms.CustomControls;
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
    public partial class PaymentForm : Form
    {
        private readonly Order _orders;
        private readonly decimal _total; 

        public PaymentForm(Order orders, decimal total)
        {
            _orders = orders;
            _total = total;
            InitializeComponent();
            InitializeOrders();
        }

        private void InitializeOrders()
        {
            _orders.Items.ForEach(order => 
                orderPanel.Controls
                    .Add(new OrderPaymentControl(order)));

            lblTotal.Text = $"{_total:#,###.00}";
        }
    }
}
