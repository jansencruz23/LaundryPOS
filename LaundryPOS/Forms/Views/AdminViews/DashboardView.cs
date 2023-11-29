using Guna.Charts.Interfaces;
using Guna.Charts.WinForms;
using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using System.Globalization;

namespace LaundryPOS.Forms.Views.AdminViews
{
    public partial class DashboardView : BaseViewControl
    {
        private readonly ISalesService _salesService;

        public DashboardView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ISalesService salesService,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, styleManager, changeAdminView)
        {
            _salesService = salesService;
            InitializeComponent();
        }

        private async void DashboardView_Load(object sender, EventArgs e)
        {
            await InitializeSalesChart();
            await InitializeSalesInfo();
        }

        private async Task InitializeSalesInfo()
        {
            lblAnnualSales.Text = $"₱ {FormatNumber(await _salesService.GetAnnualSales())}";
            lblMonthlySales.Text = $"₱ {FormatNumber(await _salesService.GetMonthlySales())}";
            lblWeeklySales.Text = $"₱ {FormatNumber(await _salesService.GetWeeklySales())}";
            lblDailySales.Text = $"₱ {FormatNumber(await _salesService.GetDailySales())}";
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
            salesChart.ApplyConfig(LightChartConfig.Config(), Color.White);
            salesChart.Datasets.Add(dataset);
            salesChart.Update();
        }

        public string FormatNumber(decimal number)
            => number >= 1000 
                ? (number / 1000).ToString("0.0") + "k"
                : number.ToString();

        private void DisplayTotalSales(string message, string title)
        {
            MessageDialog.Show(ParentForm, message, title, MessageDialogButtons.OK,
                MessageDialogIcon.Information, MessageDialogStyle.Light);
        }

        private async Task DisplaySales(string interval, Func<Task<decimal>> salesFunc)
        {
            var sales = await salesFunc();
            DisplayTotalSales($"Total {interval.ToLower()} sales: ₱ {sales}", $"Total {interval} Sales");
        }

        private async void btnAnnualSales_Click(object sender, EventArgs e)
        {
            await DisplaySales("Annual", _salesService.GetAnnualSales);
        }

        private async void btnMonthlySales_Click(object sender, EventArgs e)
        {
            await DisplaySales("Monthly", _salesService.GetMonthlySales);
        }

        private async void btnWeeklySales_Click(object sender, EventArgs e)
        {
            await DisplaySales("Weekly", _salesService.GetWeeklySales);
        }

        private async void btnDailySales_Click(object sender, EventArgs e)
        {
            await DisplaySales("Daily", _salesService.GetDailySales);
        }
    }
}