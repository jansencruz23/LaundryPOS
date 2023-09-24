using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;
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

namespace LaundryPOS.Forms.Views
{
    public partial class UnpaidForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly Employee _employee;
        private List<Employee> employeeCache;

        public UnpaidForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;
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

        private async void UnpaidForm_Load(object sender, EventArgs e)
        {
            await InitializeTable();
        }

        private async Task InitializeTable()
        {
            await LoadEmployeeData();

            var transactionItems = await _unitOfWork.TransactionItemRepo
                .Get(filter: ti => !ti.Transaction.IsCompleted, includeProperties: "Item,Transaction");

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

            unpaidTable.DataSource = groupedTransactions;
            ConfigureDataGridView(groupedTransactions);
        }

        private void ConfigureDataGridView(List<GroupedTransactionViewModel> groupedTransactions)
        {
            unpaidTable.AutoGenerateColumns = false;

            unpaidTable.Columns["EmployeeId"].Visible = false;
            unpaidTable.Columns["TransactionId"].Visible = false;
            unpaidTable.Columns["TransactionDateTime"].HeaderText = "Transaction Date";

            unpaidTable.Columns.Add("Quantity", "Quantity");
            unpaidTable.Columns.Add("Item Price", "Item Price");
            unpaidTable.Columns.Add("SubTotal", "SubTotal");

            if (unpaidTable.Columns.Contains("Total"))
            {
                DataGridViewColumn totalColumn = unpaidTable.Columns["Total"];
                totalColumn.DisplayIndex = unpaidTable.ColumnCount - 1;
            }
        }

        private void unpaidTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var columnName = unpaidTable.Columns[e.ColumnIndex].Name;

            if (columnName == "Employee")
            {
                var transaction = (GroupedTransactionViewModel)unpaidTable.Rows[e.RowIndex].DataBoundItem;
                var employee = employeeCache.FirstOrDefault(emp => emp.EmployeeId == transaction.EmployeeId);
                e.Value = employee?.FullName ?? string.Empty;
            }

            if (columnMappings.TryGetValue(columnName, out var propertySelector))
            {
                var transaction = (GroupedTransactionViewModel)unpaidTable.Rows[e.RowIndex].DataBoundItem;
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

        private void unpaidTable_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedTransaction = (GroupedTransactionViewModel)unpaidTable.Rows[e.RowIndex].DataBoundItem;
                var paymentForm = new PaymentForm(selectedTransaction.Order, selectedTransaction.Total,
                    _unitOfWork, _employee, selectedTransaction.TransactionId);

                Hide();
                paymentForm.FormClosed += (s, args) => Close();
                paymentForm.Show();
            }
        }
    }
}