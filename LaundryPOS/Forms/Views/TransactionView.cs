using LaundryPOS.Contracts;
using LaundryPOS.Models.ViewModels;
using LaundryPOS.Models;
using LaundryPOS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LaundryPOS.Forms.Views
{
    public partial class TransactionView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;   
        private readonly ThemeManager _themeManager;
        private List<Employee> employeeCache;

        public TransactionView(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            employeeCache = new();

            InitializeComponent();
        }

        private async Task LoadEmployeeData()
        {
            var employees = await _unitOfWork.EmployeeRepo.Get();

            foreach (var employee in employees)
            {
                employeeCache.Add(employee);
            }
        }

        private async void TransactionView_Load(object sender, EventArgs e)
        {
            await InitializeTable();
        }

        private async Task InitializeTable()
        {
            await LoadEmployeeData();

            var transactionItems = await _unitOfWork.TransactionItemRepo
                .Get(filter: ti => ti.Transaction.IsCompleted, includeProperties: "Item,Transaction");

            var groupedTransactions = transactionItems
                .GroupBy(ti => ti.TransactionId)
                .Select(group => new GroupedTransactionViewModel
                {
                    TransactionId = group.Key,
                    TransactionDateTime = group.First().Transaction.TransactionDate,
                    EmployeeId = group.First().Transaction.EmployeeId,
                    Order = new Order
                    {
                        Items = group.Select(ti => new CartItem(ti.Item, ti.Quantity)).ToList()
                    }
                })
                .ToList();

            transactionTable.DataSource = groupedTransactions;
            ConfigureDataGridView(groupedTransactions);
        }

        private void ConfigureDataGridView(List<GroupedTransactionViewModel> groupedTransactions)
        {
            transactionTable.AutoGenerateColumns = false;
            transactionTable.Columns["TransactionId"].Visible = false;
            transactionTable.Columns["EmployeeId"].Visible = false;
            transactionTable.Columns["TransactionDateTime"].HeaderText = "Transaction Date";

            transactionTable.Columns.Add("Quantity", "Quantity");
            transactionTable.Columns.Add("Item Price", "Item Price");
            transactionTable.Columns.Add("SubTotal", "SubTotal");

            if (transactionTable.Columns.Contains("Total"))
            {
                DataGridViewColumn totalColumn = transactionTable.Columns["Total"];
                totalColumn.DisplayIndex = transactionTable.ColumnCount - 1;
            }
        }

        private void transactionTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var columnName = transactionTable.Columns[e.ColumnIndex].Name;

            if (columnName == "Employee")
            {
                var transaction = (GroupedTransactionViewModel)transactionTable.Rows[e.RowIndex].DataBoundItem;
                var employeeId = transaction.EmployeeId;
                var employee = employeeCache.FirstOrDefault(emp => emp.EmployeeId == employeeId);
                var employeeName = employee != null ? employee.FullName : string.Empty;

                e.Value = employeeName;
            }

            if (columnMappings.TryGetValue(columnName, out var propertySelector))
            {
                var transaction = (GroupedTransactionViewModel)transactionTable.Rows[e.RowIndex].DataBoundItem;
                var columnData = propertySelector(transaction);

                e.Value = string.Join("\n", columnData);

                e.FormattingApplied = true;
            }
        }

        private Dictionary<string, Func<GroupedTransactionViewModel, IEnumerable<object>>> columnMappings = new()
        {
            { "Order", vm => vm.Order.Items.Select(item => (object)item.Item.Name) },
            { "Quantity", vm => vm.Order.Items.Select(q => (object)q.Quantity) },
            { "Item Price", vm => vm.Order.Items.Select(p => (object)p.Item.Price) },
            { "SubTotal", vm => vm.Order.Items.Select(s => (object)s.SubTotal) },
        };
    }
}
