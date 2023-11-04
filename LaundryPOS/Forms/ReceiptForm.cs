using LaundryPOS.Contracts;
using LaundryPOS.Models;
using Microsoft.Reporting.WinForms;
using System.Data;

namespace LaundryPOS.Forms
{
    public partial class ReceiptForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Transaction _transaction;

        public ReceiptForm(IUnitOfWork unitOfWork, 
            Transaction transaction)
        {
            _unitOfWork = unitOfWork;
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

<<<<<<< HEAD
            //string reportPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Reports", "Receipt.rdlc");
            reportViewer.LocalReport.ReportPath = "Reports\\Receipt.rdlc";
=======
            string reportPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Reports", "Receipt.rdlc");
            reportViewer.LocalReport.ReportPath = reportPath;
>>>>>>> 3f7bf0330be60318a516eeeef346171f49d08366
            reportViewer.LocalReport.DataSources.Add(appSettingsDataSource);
            reportViewer.LocalReport.DataSources.Add(itemDataSource);
            reportViewer.LocalReport.DataSources.Add(transactionItemDataSource);
            reportViewer.LocalReport.DataSources.Add(transactionDataSource);
            reportViewer.LocalReport.DataSources.Add(employeeDataSource);

            reportViewer.RefreshReport();
        }
    }
}