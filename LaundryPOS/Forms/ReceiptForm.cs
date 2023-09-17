using LaundryPOS.DAL;
using LaundryPOS.Models;
using Microsoft.Reporting.WinForms;
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
    public partial class ReceiptForm : Form
    {
        private readonly ReceiptDataService _dataService;
        private readonly Order _orders;
        private readonly decimal _total;

        public ReceiptForm(Order orders, decimal total)
        {
            _orders = orders;
            _total = total;
            _dataService = new();
            InitializeComponent();
            InitializeReport();
        }

        private void InitializeReport()
        {
            //var orderDataSet = _dataService.CreateReceiptDataSet(_orders.Items);
            reportViewer.LocalReport.ReportPath = "Reports/Receipt.rdlc";
            reportViewer.RefreshReport();
        }
    }
}