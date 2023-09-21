using LaundryPOS.Contracts;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly Order _orders;
        private readonly decimal _total;

        public PaymentForm(Order orders, decimal total, 
            IUnitOfWork unitOfWork)
        {
            _orders = orders;
            _total = total;
            _unitOfWork = unitOfWork;

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

        private async void btnPay_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                if (amount >= _total)
                {
                    try
                    {
                        var transaction = CreateTransaction(amount);
                        _unitOfWork.TransactionRepo.Insert(transaction);
                        await _unitOfWork.SaveAsync();

                        var receiptForm = new ReceiptForm(_unitOfWork, _orders, _total, transaction);
                        receiptForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("failed");
                }
            }
        }

        private Transaction CreateTransaction(decimal amount) => new()
        {
            EmployeeId = 1, // Replace
            TransactionDate = DateTime.Now,
            TotalAmount = _total,
            AmountPaid = amount,
            Change = amount - _total,
            IsCompleted = true,
            Items = _orders.Items.Select(order => new TransactionItem
            {
                ItemId = order.Item.ItemId,
                Quantity = order.Quantity,
                SubTotal = order.SubTotal
            }).ToList()
        };
    }
}