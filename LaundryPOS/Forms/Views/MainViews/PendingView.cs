using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;
using System.Linq.Expressions;

namespace LaundryPOS.Forms.Views
{
    public partial class PendingView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;
        private readonly List<Employee> _employeeCache;

        #region -- TIME CONSTANTS --
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

        public PendingView(IUnitOfWork unitOfWork,
            IStyleManager styleManager)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _employeeCache = new();

            InitializeComponent();
        }

        private async Task LoadEmployeeData()
        {
            var employees = await _unitOfWork.EmployeeRepo.Get();

            foreach (var employee in employees)
                _employeeCache.Add(employee);
        }

        private async void PendingView_Load(object sender, EventArgs e)
        {
            var filter = cbFilter.SelectedItem?.ToString()!;
            var expression = GetDateExpression(filter);
            await DisplayPending(expression);
            await ApplyTheme();
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyLighterThemeToDataGridView(unpaidTable, changeFont: true);
            await _styleManager.Theme.ApplyThemeToButton(btnSearch);
        }

        private async Task DisplayPending(Expression<Func<TransactionItem, bool>> filter)
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

            unpaidTable.DataSource = groupedTransactions;
            ConfigureDataGridView(groupedTransactions);
        }

        private void ConfigureDataGridView(List<GroupedTransactionViewModel> groupedTransactions)
        {
            if (unpaidTable.Columns["SubTotal"] == null)
            {
                unpaidTable.AutoGenerateColumns = false;

                unpaidTable.Columns["EmployeeId"].Visible = false;
                unpaidTable.Columns["TransactionDateTime"].HeaderText = "Transaction Date";
                unpaidTable.Columns["TransactionId"].HeaderText = "Transaction #";

                unpaidTable.Columns.Add("Quantity", "Quantity");
                unpaidTable.Columns.Add("Item Price", "Item Price");
                unpaidTable.Columns.Add("SubTotal", "SubTotal");

                if (unpaidTable.Columns.Contains("Total"))
                {
                    DataGridViewColumn totalColumn = unpaidTable.Columns["Total"];
                    totalColumn.DisplayIndex = unpaidTable.ColumnCount - 1;
                }

                var payBtn = new DataGridViewButtonColumn
                {
                    Name = "Pay Now",
                    Text = "Pay Now",
                    UseColumnTextForButtonValue = true
                };

                payBtn.DefaultCellStyle.ForeColor = Color.White;
                payBtn.FlatStyle = FlatStyle.Flat;
                payBtn.DefaultCellStyle.SelectionForeColor = Color.White;
                payBtn.DefaultCellStyle.SelectionBackColor = Color.Black;

                int columnIndex = unpaidTable.ColumnCount;
                if (unpaidTable.Columns["Pay Now"] == null)
                {
                    unpaidTable.Columns.Insert(columnIndex, payBtn);
                }
            }
        }

        private void unpaidTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var columnName = unpaidTable.Columns[e.ColumnIndex].Name;

            if (columnName == "Employee")
            {
                var transaction = (GroupedTransactionViewModel)unpaidTable.Rows[e.RowIndex].DataBoundItem;
                var employee = _employeeCache.FirstOrDefault(emp => emp.Id == transaction.EmployeeId);
                e.Value = employee?.FullName ?? string.Empty;
            }

            if (columnMappings.TryGetValue(columnName, out var propertySelector))
            {
                var transaction = (GroupedTransactionViewModel)unpaidTable.Rows[e.RowIndex].DataBoundItem;
                var columnData = propertySelector(transaction);

                e.Value = string.Join("\n", columnData);
                e.FormattingApplied = true;
            }

            if (columnName == "Pay Now")
            {
                e.CellStyle.BackColor = ColorTranslator.FromHtml("#303030");
            }
        }

        private Dictionary<string, Func<GroupedTransactionViewModel, IEnumerable<object>>> columnMappings = new()
        {
            { "Order", vm => vm.Order.Items.Select(item => (object)item.Item.Name) },
            { "Quantity", vm => vm.Order.Items.Select(q => (object)q.Quantity) },
            { "Item Price", vm => vm.Order.Items.Select(p => (object)p.Item.Price) },
            { "SubTotal", vm => vm.Order.Items.Select(s => (object)s.SubTotal) },
        };

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await RefreshData(txtSearch.Text);
        }

        private async Task RefreshData(string query = "")
        {
            await DisplayPending(ti => !ti.Transaction.IsCompleted 
                && ti.TransactionId.ToString().Contains(query));
        }

        private async void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filter = cbFilter.SelectedItem?.ToString()!;

            if (string.IsNullOrEmpty(filter))
            {
                MessageDialog.Show(ParentForm, "Invalid filter", "Filter Failed",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                return;
            }

            var filterPredicate = GetDateExpression(filter);
            await DisplayPending(filterPredicate);
        }

        private Expression<Func<TransactionItem, bool>> GetDateExpression(string filter)
        {
            DateTime startDate = default;
            DateTime endDate = default;
            Expression<Func<TransactionItem, bool>> filterPredicate = ti => !ti.Transaction.IsCompleted;

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
                    MessageDialog.Show(ParentForm, "Invalid filter", "Filter Failed",
                        MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                    break;
            }

            if (filter != "All")
            {
                return ti => ti.Transaction.TransactionDate >= startDate
                    && ti.Transaction.TransactionDate <= endDate
                    && !ti.Transaction.IsCompleted;
            }

            return filterPredicate;
        }

        private async void unpaidTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (Guna2DataGridView) sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var selectedTransaction = (GroupedTransactionViewModel)unpaidTable.Rows[e.RowIndex].DataBoundItem;
                var paymentForm = new PaymentForm(selectedTransaction.Order, selectedTransaction.Total,
                    _unitOfWork, _employee, _styleManager, selectedTransaction.TransactionId);

                paymentForm.ShowDialog();
                await RefreshDataDaily();
            }
        }

        private async Task RefreshDataDaily()
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(NEXT_DAY);

            await DisplayPending(ti => !ti.Transaction.IsCompleted
                && ti.Transaction.TransactionDate >= startDate
                && ti.Transaction.TransactionDate <= endDate);
        }
    }
}