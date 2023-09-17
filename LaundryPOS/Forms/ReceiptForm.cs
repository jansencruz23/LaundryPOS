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

        public ReceiptForm(IUnitOfWork unitOfWork, 
            Order orders, decimal total)
        {
            _unitOfWork = unitOfWork;
            _orders = orders;
            _total = total;
            _dataService = new();
            InitializeComponent();
            InitializeReport();
        }

        private async void InitializeReport()
        {
            //var orderDataSet = _dataService.CreateReceiptDataSet(_orders.Items);

            var appSettingsData = await _unitOfWork.AppSettingsRepo.Get();
            var itemData = await _unitOfWork.ItemRepo.Get();
            ReportDataSource dataSource = new ReportDataSource("ShopInfoDataSet", appSettingsData);
            ReportDataSource dataSource2 = new ReportDataSource("ItemDataSet", itemData);
            
            reportViewer.LocalReport.ReportPath = "Reports/Receipt.rdlc";
            reportViewer.LocalReport.DataSources.Add(dataSource);
            reportViewer.LocalReport.DataSources.Add(dataSource2);
            reportViewer.RefreshReport();
        }
    }
}