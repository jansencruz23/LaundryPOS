using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;

namespace LaundryPOS.Forms.Views
{
    public partial class PendingView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;
        private readonly List<Employee> _employeeCache;

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
            await InitializeTable();
            await ApplyTheme();
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyLighterThemeToDataGridView(unpaidTable, changeFont: true);
            await _styleManager.Theme.ApplyThemeToButton(btnSearch);
        }

        private async Task InitializeTable(string query = "")
        {
            await LoadEmployeeData();

            var transactionItems = await _unitOfWork.TransactionItemRepo
                .Get(filter: ti => !ti.Transaction.IsCompleted
                    && (ti.TransactionId.ToString().Contains(query)
                    || ti.Transaction.Employee.LastName.Contains(query)
                    || ti.Transaction.Employee.FirstName.Contains(query)),
                includeProperties: "Item,Transaction");

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

                unpaidTable.Columns.Add("Quantity", "Quantity");
                unpaidTable.Columns.Add("Item Price", "Item Price");
                unpaidTable.Columns.Add("SubTotal", "SubTotal");

                if (unpaidTable.Columns.Contains("Total"))
                {
                    DataGridViewColumn totalColumn = unpaidTable.Columns["Total"];
                    totalColumn.DisplayIndex = unpaidTable.ColumnCount - 1;
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
                    _unitOfWork, _employee, _styleManager, selectedTransaction.TransactionId);

                paymentForm.ShowDialog();
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await RefreshData(txtSearch.Text);
        }

        private async Task RefreshData(string query = "")
        {
            await InitializeTable(query);
        }
    }
}