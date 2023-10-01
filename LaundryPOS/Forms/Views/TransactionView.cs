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
using LaundryPOS.Delegates;
using System.Linq.Expressions;

namespace LaundryPOS.Forms.Views
{
    public partial class TransactionView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;   
        private readonly ThemeManager _themeManager;
        private readonly ChangeAdminViewDelegate _changeAdminView;
        private List<Employee> employeeCache;

        private const int DAYS_IN_WEEK = 7;
        private const int NEXT_DAY = 1;
        private const int FIRST_DAY = 1;
        private const int FIRST_MONTH = 1;
        private const int LAST_MONTH = 12;
        private const int LAST_DAY = 31;

        public TransactionView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;
            employeeCache = new();

            InitializeComponent();
        }

        private async Task LoadEmployeeData()
        {
            var employees = await _unitOfWork.EmployeeRepo.Get();

            foreach (var employee in employees)
                employeeCache.Add(employee);
        }

        private async void TransactionView_Load(object sender, EventArgs e)
        {
            await DisplayTransactions(ti => ti.Transaction.IsCompleted);
            await ApplyTheme();
        }

        private async Task DisplayTransactions(Expression<Func<TransactionItem, bool>> filter)
        {
            await LoadEmployeeData();

            var transactionItems = await _unitOfWork.TransactionItemRepo
                .Get(filter: filter, includeProperties: "Item,Transaction",
                    orderBy: ti => ti.OrderByDescending(ti => ti.Transaction.TransactionDate));

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

            if (transactionTable.Columns["Quantity"] == null)
                ConfigureDataGridView(groupedTransactions);
        }

        private void ConfigureDataGridView(List<GroupedTransactionViewModel> groupedTransactions)
        {
            transactionTable.AutoGenerateColumns = false;

            transactionTable.Columns["EmployeeId"].Visible = false;
            transactionTable.Columns["TransactionId"].Visible = false;
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
                var employee = employeeCache.FirstOrDefault(emp => emp.EmployeeId == transaction.EmployeeId);
                e.Value = employee?.FullName ?? string.Empty;
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

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnTransaction);
            await _themeManager.ApplyLighterThemeToDataGridView(transactionTable);
        }

        private void ChangeAdminView<T>(Func<IUnitOfWork, ThemeManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _themeManager, _changeAdminView);
            _changeAdminView?.Invoke(view);
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new ItemView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new CategoryView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new EmployeeView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new AdminProfileView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private async void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = cbFilter.SelectedItem.ToString();

            if (filter == null)
            {
                MessageBox.Show("Invalid filter");
                return;
            }
                
            switch (filter)
            {
                case "All":
                    await DisplayTransactions(filter: ti => ti.Transaction.IsCompleted);
                    break;

                case "Daily":
                    var today = DateTime.Today;
                    var tomorrow = today.AddDays(NEXT_DAY);
                    await DisplayTransactions(filter: ti => ti.Transaction.TransactionDate > today
                        && ti.Transaction.TransactionDate < tomorrow
                        && ti.Transaction.IsCompleted);
                    break;

                case "Weekly":
                    var startWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    var endWeek = startWeek.AddDays(DAYS_IN_WEEK).AddSeconds(-1);
                    await DisplayTransactions(filter: ti => ti.Transaction.TransactionDate <= endWeek
                        && ti.Transaction.IsCompleted);
                    break;

                case "Monthly":
                    var startMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    var endMonth = startMonth.AddMonths(FIRST_DAY).AddSeconds(-1);
                    await DisplayTransactions(filter: ti => ti.Transaction.TransactionDate >= startMonth
                        && ti.Transaction.TransactionDate <= endMonth
                        && ti.Transaction.IsCompleted);
                    break;

                case "Yearly":
                    var startYear = new DateTime(DateTime.Today.Year, FIRST_MONTH, FIRST_DAY);
                    var endYear = new DateTime(DateTime.Today.Year, LAST_MONTH, LAST_DAY, 23, 59, 59);
                    await DisplayTransactions(filter: ti => ti.Transaction.TransactionDate >= startYear
                        && ti.Transaction.TransactionDate <= endYear
                        && ti.Transaction.IsCompleted);
                    break;

                default:
                    MessageBox.Show("Invalid filter");
                    break;
            }
        }
    }
}