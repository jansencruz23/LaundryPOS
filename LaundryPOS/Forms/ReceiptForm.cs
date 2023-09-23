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
            var transactionId = _transaction.TransactionId;

            var appSettingsData = await _unitOfWork.AppSettingsRepo.Get();
            var itemData = await _unitOfWork.ItemRepo.GetBoughtItems(transactionId);
            var transactionItemData = await _unitOfWork.TransactionItemRepo.GetTransactionItems(transactionId);
            var transactionData = await _unitOfWork.TransactionRepo.Get(
                filter: t => t.TransactionId == transactionId,
                orderBy: t => t.OrderBy(t => t.TransactionId));
            var employeeData = await _unitOfWork.EmployeeRepo.GetTransactedEmployee(transactionId);

            var appSettingsDataSource = new ReportDataSource("ShopInfoDataSet", appSettingsData);
            var itemDataSource = new ReportDataSource("ItemDataSet", itemData);
            var transactionItemDataSource = new ReportDataSource("TransactionItemsDataSet", transactionItemData);
            var employeeDataSource = new ReportDataSource("EmployeeDataSet", employeeData);
            var transactionDataSource = new ReportDataSource("TransactionDataSet", transactionData);
            
            reportViewer.LocalReport.ReportPath = "Reports/Receipt.rdlc";
            reportViewer.LocalReport.DataSources.Add(appSettingsDataSource);
            reportViewer.LocalReport.DataSources.Add(itemDataSource);
            reportViewer.LocalReport.DataSources.Add(transactionItemDataSource);
            reportViewer.LocalReport.DataSources.Add(transactionDataSource);
            reportViewer.LocalReport.DataSources.Add(employeeDataSource);

            reportViewer.RefreshReport();
        }
    }
}