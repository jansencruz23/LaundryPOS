using LaundryPOS.Contracts;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ReceiptDataService _dataService;
        private readonly Order _orders;
        private readonly decimal _total;
        private readonly Transaction _transaction;

        public ReceiptForm(IUnitOfWork unitOfWork, 
            Order orders, decimal total,
            Transaction transaction)
        {
            _unitOfWork = unitOfWork;
            _orders = orders;
            _total = total;
            _dataService = new();
            _transaction = transaction;

            InitializeComponent();
            InitializeReport();
        }

        private async void InitializeReport()
        {
            //var orderDataSet = _dataService.CreateReceiptDataSet(_orders.Items);

            var appSettingsData = await _unitOfWork.AppSettingsRepo.Get();
            var itemData = await _unitOfWork.ItemRepo.Get();
            var transactionData = await _unitOfWork.TransactionItemRepo.GetTransactionItems(_transaction.TransactionId);

            ReportDataSource appSettingsDataSource = new ReportDataSource("ShopInfoDataSet", appSettingsData);
            ReportDataSource itemDataSource = new ReportDataSource("ItemDataSet", itemData);
            ReportDataSource transactionDataSource = new ReportDataSource("TransactionItemsDataSet", transactionData);
            
            reportViewer.LocalReport.ReportPath = "Reports/Receipt.rdlc";
            reportViewer.LocalReport.DataSources.Add(appSettingsDataSource);
            reportViewer.LocalReport.DataSources.Add(itemDataSource);
            reportViewer.LocalReport.DataSources.Add(transactionDataSource);

            reportViewer.RefreshReport();
        }
    }
}