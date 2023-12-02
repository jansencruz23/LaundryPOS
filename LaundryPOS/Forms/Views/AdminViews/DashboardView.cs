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
            await InitializeDailySalesChart();
            await InitializeSalesInfo();
        }

        private async Task InitializeSalesInfo()
        {
            lblAnnualSales.Text = $"₱ {FormatNumber(await _salesService.GetAnnualSales())}";
            lblMonthlySales.Text = $"₱ {FormatNumber(await _salesService.GetMonthlySales())}";
            lblWeeklySales.Text = $"₱ {FormatNumber(await _salesService.GetWeeklySales())}";
            lblDailySales.Text = $"₱ {FormatNumber(await _salesService.GetDailySales())}";
        }

        private async Task InitializeWeeklySalesChart()
        {
            var startDate = DateTime.Today.AddMonths(-1);
            var endDate = DateTime.Today.AddDays(1);
            var sales = await _salesService.GetSales(startDate, endDate);
            var salesData = sales
                .GroupBy(ti => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    ti.Transaction.TransactionDate,
                    CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Sunday
                ))
                .Select(group => new
                {
                    WeekNumber = group.Key,
                    TotalSales = group.Sum(ti => ti.SubTotal)
                })
               .ToList();

            var dataset = new GunaSplineDataset();
            var currentWeekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);

            for (int i = 7; i >= 0; i--)
            {
                int week = currentWeekOfYear - i;
                if (week <= 0)
                {
                    week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                        DateTime.Today.AddDays(-i * 7), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
                }

                var startDateLabel = GetStartWeekDate(DateTime.Today.Year, week);
                var endDateLabel = GetEndWeekDate(DateTime.Today.Year, week);
                var salesForWeek = salesData.FirstOrDefault(d => d.WeekNumber == week);
                var salesAmount = (double?)salesForWeek?.TotalSales ?? 0;
                var formattedDate = $"{startDateLabel:MMM d} - {endDateLabel:MMM d}";

                dataset.DataPoints.Add(formattedDate, salesAmount);
            }

            ApplyChartConfig(dataset, "Weekly");
        }

        private async Task InitializeMonthlySalesChart()
        {
            var startDate = DateTime.Today.AddMonths(-12);
            var endDate = DateTime.Today.AddDays(1);
            var sales = await _salesService.GetSales(startDate, endDate);
            var salesData = sales
                .GroupBy(ti => ti.Transaction.TransactionDate.Month)
                .Select(group => new
                {
                    MonthNumber = group.Key,
                    TotalSales = group.Sum(ti => ti.SubTotal)
                })
               .ToList();

            var dataset = new GunaSplineDataset();

            var currentMonth = DateTime.Today.Month;
            for (int i = 7; i >= 0; i--)
            {
                int month = currentMonth - i;
                if (month <= 0)
                {
                    month = 12 + month;
                }
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                var salesForMonth = salesData.FirstOrDefault(d => d.MonthNumber == month);
                var salesAmount = (double?)salesForMonth?.TotalSales ?? 0;
                var formattedDate = $"{monthName}";

                dataset.DataPoints.Add(formattedDate, salesAmount);
            }

            ApplyChartConfig(dataset, "Monthly");
        }

        private async Task InitializeAnnualSalesChart()
        {
            var startDate = DateTime.Today.AddYears(-7);
            var endDate = DateTime.Today.AddDays(1);
            var sales = await _salesService.GetSales(startDate, endDate);
            var salesData = sales
                .GroupBy(ti => ti.Transaction.TransactionDate.Year)
                .Select(group => new
                {
                    Year = group.Key,
                    TotalSales = group.Sum(ti => ti.SubTotal)
                })
               .ToList();

            var dataset = new GunaSplineDataset();
            var currentYear = DateTime.Today.Year;
            for (int i = 7; i >= 0; i--)
            {
                int year = currentYear - i;
                if (year <= 0)
                {
                    year = 12 + year;
                }

                var salesForYear = salesData.FirstOrDefault(d => d.Year == year);
                var salesAmount = (double?)salesForYear?.TotalSales ?? 0;
                var formattedDate = $"{year}";

                dataset.DataPoints.Add(formattedDate, salesAmount);
            }

            ApplyChartConfig(dataset, "Annual");
        }

        private async Task InitializeDailySalesChart()
        {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(1);
            var sales = await _salesService.GetSales(startDate, endDate);

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

            ApplyChartConfig(dataset, "Daily");
        }

        private void ApplyChartConfig(GunaSplineDataset dataset, string title)
        {
            salesChart.Reset();
            dataset.Label = $"{title} Sales";
            salesChart.ApplyConfig(LightChartConfig.Config(), Color.White);
            salesChart.Datasets.Add(dataset);
            salesChart.Update();
            lblStatsTitle.Text = $"{title} Sales Statistics";
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

        private async void cbSalesChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            var chartText = cbSalesChart.SelectedItem.ToString();

            switch (chartText)
            {
                case "Daily":
                    await InitializeDailySalesChart();
                    break;
                case "Weekly":
                    await InitializeWeeklySalesChart();
                    break;
                case "Monthly":
                    await InitializeMonthlySalesChart();
                    break;
                case "Annually":
                    await InitializeAnnualSalesChart();
                    break;
            }
        }

        private DateTime GetStartWeekDate(int year, int weekNumber)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Monday - jan1.DayOfWeek + 1;
            var firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            var firstWeek = cal.GetWeekOfYear(firstMonday, 
                CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            if (firstWeek <= 1)
            {
                weekNumber -= 1;
            }

            return firstMonday.AddDays(weekNumber * 7);
        }

        private DateTime GetEndWeekDate(int year, int weekNumber)
        {
            return GetStartWeekDate(year, weekNumber).AddDays(6);
        }
    }
}