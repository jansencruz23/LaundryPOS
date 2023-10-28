﻿using LaundryPOS.Contracts;
using LaundryPOS.Models.ViewModels;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using System.Data;
using LaundryPOS.Delegates;
using System.Linq.Expressions;

namespace LaundryPOS.Forms.Views
{
    public partial class TransactionView : BaseViewControl
    {
        private readonly List<Employee> _employeeCache;

        #region -- CONSTANTS --
        private const int DAYS_IN_WEEK = 7;
        private const int NEXT_DAY = 1;
        private const int NEXT_MONTH = 1;
        private const int FIRST_DAY = 1;
        private const int FIRST_MONTH = 1;
        private const int LAST_MONTH = 12;
        private const int LAST_DAY = 31;
        private const int LAST_HOUR = 23;
        private const int LAST_MINUTE = 59;
        private const int LAST_SECOND = 59;
        #endregion

        public TransactionView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, themeManager, changeAdminView)
        {
            _employeeCache = new();
            InitializeComponent();
        }

        private async Task LoadEmployeeData()
        {
            var employees = await _unitOfWork.EmployeeRepo.Get();

            foreach (var employee in employees)
                _employeeCache.Add(employee);
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

        private async Task RefreshData(string query = "")
        {       
            await DisplayTransactions(ti => ti.Transaction.IsCompleted 
                && ti.TransactionId.ToString().Contains(query));
        }

        private void ConfigureDataGridView(List<GroupedTransactionViewModel> groupedTransactions)
        {
            transactionTable.AutoGenerateColumns = false;

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
                var employee = _employeeCache.FirstOrDefault(emp => emp.Id == transaction.EmployeeId);
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

        private readonly Dictionary<string, Func<GroupedTransactionViewModel, IEnumerable<object>>> columnMappings = new()
        {
            { "Order", vm => vm.Order.Items.Select(item => (object)item.Item.Name) },
            { "Quantity", vm => vm.Order.Items.Select(q => (object)q.Quantity) },
            { "Item Price", vm => vm.Order.Items.Select(p => (object)p.Item.Price) },
            { "SubTotal", vm => vm.Order.Items.Select(s => (object)s.SubTotal) },
        };

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnTransaction);
            await _themeManager.ApplyThemeToButton(btnSearch);
            await _themeManager.ApplyLighterThemeToDataGridView(transactionTable, 1f, true);
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<ItemView>());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<CategoryView>());
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<EmployeeView>());
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<AdminProfileView>());
        }

        private async void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filter = cbFilter.SelectedItem?.ToString()!;

            if (string.IsNullOrEmpty(filter))
            {
                MessageBox.Show("Invalid filter");
                return;
            }

            var filterPredicate = GetDateExpression(filter);
            await DisplayTransactions(filterPredicate);
        }

        private Expression<Func<TransactionItem, bool>> GetDateExpression(string filter)
        {
            DateTime startDate = default;
            DateTime endDate = default;
            Expression<Func<TransactionItem, bool>> filterPredicate = ti => ti.Transaction.IsCompleted;

            switch (filter)
            {
                case "Daily":
                    startDate = DateTime.Today;
                    endDate = startDate.AddDays(NEXT_DAY);
                    break;

                case "Weekly":
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    endDate = startDate.AddDays(DAYS_IN_WEEK);
                    break;

                case "Monthly":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = startDate.AddMonths(NEXT_MONTH);
                    break;

                case "Yearly":
                    startDate = new DateTime(DateTime.Today.Year, FIRST_MONTH, FIRST_DAY);
                    endDate = new DateTime(DateTime.Today.Year, LAST_MONTH, LAST_DAY, LAST_HOUR, LAST_MINUTE, LAST_SECOND);
                    break;

                case "All":
                    break;

                default:
                    MessageBox.Show("Invalid filter");
                    break;
            }

            if (filter != "All")
            {
                return ti => ti.Transaction.TransactionDate >= startDate
                    && ti.Transaction.TransactionDate <= endDate
                    && ti.Transaction.IsCompleted;
            }

            return filterPredicate;
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            var printer = new DGVPrinter();
            var businessName = await GetBusinessName();

            printer.Title = businessName;
            printer.SubTitle = string.Format("Transaction List", printer.SubTitleColor = Color.Black, printer);
            printer.SubTitleSpacing = 15;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.RowHeight = DGVPrinter.RowHeightSetting.CellHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = businessName;
            printer.FooterSpacing = 15;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
            printer.TableAlignment = DGVPrinter.Alignment.Center;
            printer.PrintDataGridView(transactionTable);
        }

        private async Task<string> GetBusinessName() =>
            (await _unitOfWork.AppSettingsRepo.GetByID(1)).Name;

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ConfirmLogout();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await RefreshData(txtSearch.Text);
        }
    }
}