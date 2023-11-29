using Guna.Charts.WinForms;
using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using System.Globalization;

namespace LaundryPOS.Forms.Views.AdminViews
{
    public partial class DashboardView : BaseViewControl
    {
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

        public DashboardView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, styleManager, changeAdminView)
        {
            InitializeComponent();
        }

        private async void DashboardView_Load(object sender, EventArgs e)
        {
            await InitializeSalesChart();
            await InitializeSalesInfo();
        }

        private async Task InitializeSalesInfo()
        {
            var annualSales = await GetAnnualSales();
            lblAnnualSales.Text = $"₱ {FormatNumber(annualSales)}";
            
            var monthlySales = await GetMonthlySales();
            lblMonthlySales.Text = $"₱ {FormatNumber(monthlySales)}";

            var weeklySales = await GetWeeklySales();
            lblWeeklySales.Text = $"₱ {FormatNumber(weeklySales)}";

            var dailySales = await GetDailySales();
            lblDailySales.Text = $"₱ {FormatNumber(dailySales)}";
        }

        private async Task InitializeSalesChart()
        {
            salesChart.YAxes.GridLines.Display = false;

            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(1);

            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

            var salesData = sales
                .GroupBy(ti => new
                {
                    ti.Transaction.TransactionDate.Date
                })
                .Select(group => new
                {
                    group.Key.Date,
                    TotalSales = group.Sum(ti => ti.SubTotal)
                })
               .ToList();

            var dataset = new GunaSplineDataset();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var salesForDate = salesData.FirstOrDefault(d => d.Date == date.Date);
                var formattedDate = date.ToString("MMM dd", CultureInfo.InvariantCulture);
                var salesAmount = (double?)salesForDate?.TotalSales ?? 0;

                dataset.DataPoints.Add(formattedDate, salesAmount);
                dataset.YFormat = $"₱{salesAmount:N2}";
            }

            dataset.Label = "Weekly Sales";

            salesChart.Datasets.Add(dataset);
            salesChart.Update();
        }

        private async Task<decimal> GetAnnualSales()
        {
            var startDate = new DateTime(DateTime.Today.Year, FIRST_MONTH, FIRST_DAY);
            var endDate = new DateTime(DateTime.Today.Year, LAST_MONTH, LAST_DAY, LAST_HOUR, LAST_MINUTE, LAST_SECOND);

            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

            return sales.Sum(sale => sale.SubTotal);
        }

        private async Task<decimal> GetMonthlySales()
        {
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endDate = startDate.AddMonths(NEXT_MONTH);

            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

            return sales.Sum(sale => sale.SubTotal);
        }

        private async Task<decimal> GetWeeklySales()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endDate = startDate.AddDays(DAYS_IN_WEEK);

            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

            return sales.Sum(sale => sale.SubTotal);
        }

        private async Task<decimal> GetDailySales()
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(NEXT_DAY);

            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

            return sales.Sum(sale => sale.SubTotal);
        }

        public string FormatNumber(decimal number)
            => number >= 1000 
                ? (number / 1000).ToString("0.0") + "k"
                : number.ToString();

        private async void panelAnnualSales_Click(object sender, EventArgs e)
        {

        }

        private void DisplayTotalSales(string message, string title)
        {
            MessageDialog.Show(ParentForm, message, title, MessageDialogButtons.OK,
                MessageDialogIcon.Information, MessageDialogStyle.Light);
        }

        private async void btnAnnualSales_Click(object sender, EventArgs e)
        {
            var annualSales = await GetAnnualSales();
            DisplayTotalSales($"Total annual sales: ₱ {annualSales}",
                "Total Annual Sales");
        }

        private async void btnMonthlySales_Click(object sender, EventArgs e)
        {
            var monthlySales = await GetMonthlySales();
            DisplayTotalSales($"Total monthly sales: ₱ {monthlySales}",
                "Total Monthly Sales");
        }

        private async void btnWeeklySales_Click(object sender, EventArgs e)
        {
            var weeklySales = await GetWeeklySales();
            DisplayTotalSales($"Total weekly sales: ₱ {weeklySales}",
                "Total Weekly Sales");
        }

        private async void btnDailySales_Click(object sender, EventArgs e)
        {
            var dailySales = await GetDailySales();
            DisplayTotalSales($"Total daily sales: ₱ {dailySales}",
                "Total Daily Sales");
        }
    }
}