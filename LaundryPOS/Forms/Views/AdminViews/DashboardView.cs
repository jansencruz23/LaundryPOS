using Guna.Charts.Interfaces;
using Guna.Charts.WinForms;
using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using LaundryPOS.Models.ViewModels;
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
            panelBody.AutoScroll = true;
        }

        private async void DashboardView_Load(object sender, EventArgs e)
        {
            await InitializeSalesInfo();
            await ShowWeeklySalesChart();
            await ShowWeekTopItems();
            await ShowWeekTopCategories();
            await ShowWeekTopEmployee();
        }

        private async Task InitializeSalesInfo()
        {
            lblAnnualSales.Text = $"₱ {FormatNumber(await _salesService.GetAnnualSales())}";
            lblMonthlySales.Text = $"₱ {FormatNumber(await _salesService.GetMonthlySales())}";
            lblWeeklySales.Text = $"₱ {FormatNumber(await _salesService.GetWeeklySales())}";
            lblDailySales.Text = $"₱ {FormatNumber(await _salesService.GetDailySales())}";
        }

        private async Task ShowWeeklySalesChart()
        {
            var startDate = DateTime.Today.AddMonths(-1);
            var endDate = DateTime.Today.AddDays(1);

            var salesData = await _salesService.GetWeeklySalesChartData(startDate, endDate);
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
                var salesForWeek = salesData.FirstOrDefault(d => d.DateNumber == week + 1);
                var salesAmount = (double?)salesForWeek?.Sales ?? 0;
                var formattedDate = $"{startDateLabel:MMM d} - {endDateLabel:MMM d}";

                dataset.DataPoints.Add(formattedDate, salesAmount);
            }

            ApplyChartConfig(dataset, "Weekly");
        }

        private async Task InitializeMonthlySalesChart()
        {
            var startDate = DateTime.Today.AddMonths(-12);
            var endDate = DateTime.Today.AddDays(1);

            var salesData = await _salesService.GetMonthlySalesChartData(startDate, endDate);
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
                var salesForMonth = salesData.FirstOrDefault(d => d.DateNumber == month);
                var salesAmount = (double?)salesForMonth?.Sales ?? 0;
                var formattedDate = $"{monthName}";

                dataset.DataPoints.Add(formattedDate, salesAmount);
            }

            ApplyChartConfig(dataset, "Monthly");
        }

        private async Task InitializeAnnualSalesChart()
        {
            var startDate = DateTime.Today.AddYears(-7);
            var endDate = DateTime.Today.AddDays(1);

            var salesData = await _salesService.GetAnnualSalesChartData(startDate, endDate);
            var dataset = new GunaSplineDataset();
            var currentYear = DateTime.Today.Year;

            for (int i = 7; i >= 0; i--)
            {
                int year = currentYear - i;
                if (year <= 0)
                {
                    year = 12 + year;
                }

                var salesForYear = salesData.FirstOrDefault(d => d.DateNumber == year);
                var salesAmount = (double?)salesForYear?.Sales ?? 0;
                var formattedDate = $"{year}";

                dataset.DataPoints.Add(formattedDate, salesAmount);
            }

            ApplyChartConfig(dataset, "Annual");
        }

        private async Task InitializeDailySalesChart()
        {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(1);

            var salesData = await _salesService.GetDailySalesChartData(startDate, endDate);
            var dataset = new GunaSplineDataset();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var salesForDate = salesData.FirstOrDefault(d => d.Date == date.Date);
                var formattedDate = date.ToString("MMM dd", CultureInfo.InvariantCulture);
                var salesAmount = (double?)salesForDate?.Sales ?? 0;

                dataset.DataPoints.Add(formattedDate, salesAmount);
            }

            ApplyChartConfig(dataset, "Daily");
        }

        private void ApplyChartConfig(GunaSplineDataset dataset, string title)
        {
            chartSales.Reset();
            dataset.Label = $"{title} Sales";
            chartSales.ApplyConfig(LightChartConfig.Config(), Color.White);
            chartSales.Datasets.Add(dataset);
            chartSales.Update();
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
                    await ShowWeeklySalesChart();
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
            var daysOffset = DayOfWeek.Monday - jan1.DayOfWeek - 1;
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

        private async Task ConfigItemsChart(DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var sales = await _salesService.GetSales(startDate, endDate);
                var topItems = sales
                    .GroupBy(item => item.ItemId)
                    .Select(group => new
                    {
                        ItemId = group.Key,
                        Quantity = group.Sum(item => item.Quantity),
                    })
                    .OrderByDescending(item => item.Quantity)
                    .Take(10)
                    .ToList();

                var dataset = new GunaHorizontalBarDataset();
                var items = await _unitOfWork.ItemRepo.Get();

                foreach (var item in topItems)
                {
                    var itemName = items.FirstOrDefault(i => i.Id == item.ItemId)?.Name;
                    if (!string.IsNullOrEmpty(itemName))
                    {
                        dataset.DataPoints.Add(itemName, item.Quantity);
                    }
                }

                chartTopItems.Reset();
                dataset.Label = "Sold Items";
                chartTopItems.ApplyConfig(LightChartConfig.Config(), Color.White);
                chartTopItems.Datasets.Add(dataset);
                chartTopItems.Update();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ParentForm, ex.Message, "An Error Occurred",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
            }
        }

        private async void cbTopItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            var chartText = cbTopItems.SelectedItem.ToString();

            switch (chartText)
            {
                case "Daily":
                    await InitializeDayTopItems();
                    break;
                case "Weekly":
                    await ShowWeekTopItems();
                    break;
                case "Monthly":
                    await InitializeMonthTopItems();
                    break;
                case "Annually":
                    await InitializeAnnualTopItems();
                    break;
            }
        }

        private async Task InitializeDayTopItems()
        {
            var startDate = DateTime.Today.AddDays(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigItemsChart(startDate, endDate);
        }

        private async Task ShowWeekTopItems()
        {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigItemsChart(startDate, endDate);
        }

        private async Task InitializeMonthTopItems()
        {
            var startDate = DateTime.Today.AddMonths(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigItemsChart(startDate, endDate);
        }

        private async Task InitializeAnnualTopItems()
        {
            var startDate = DateTime.Today.AddYears(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigItemsChart(startDate, endDate);
        }

        private async Task ConfigCategoryChart(DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var sales = await _salesService.GetSales(startDate, endDate);

                var categoryQuantities = sales
                    .GroupBy(item => item.Item.CategoryId)
                    .Select(group => new
                    {
                        Id = group.Key,
                        Quantity = group.Sum(item => item.Quantity)
                    })
                    .OrderByDescending(category => category.Quantity)
                    .ToList();

                var dataset = new GunaPieDataset();
                var categories = await _unitOfWork.CategoryRepo.Get();

                foreach (var category in categoryQuantities)
                {
                    var categoryName = categories.FirstOrDefault(i => i.Id == category.Id)?.Name;
                    dataset.DataPoints.Add(categoryName, category.Quantity);
                }

                chartCategory.Reset();
                chartCategory.ApplyConfig(LightChartConfig.PieChartConfig(), Color.White);
                chartCategory.Datasets.Add(dataset);
                chartCategory.XAxes.Display = false;
                chartCategory.YAxes.Display = false;
                chartCategory.Legend.Position = LegendPosition.Right;
                chartCategory.Update();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ParentForm, ex.Message, "An Error Occurred",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
            }
        }

        private async Task ShowDayTopCategories()
        {
            var startDate = DateTime.Today.AddDays(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigCategoryChart(startDate, endDate);
        }

        private async Task ShowWeekTopCategories()
        {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigCategoryChart(startDate, endDate);
        }

        private async Task ShowMonthTopCategories()
        {
            var startDate = DateTime.Today.AddMonths(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigCategoryChart(startDate, endDate);
        }

        private async Task ShowAnnualTopCategories()
        {
            var startDate = DateTime.Today.AddYears(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigCategoryChart(startDate, endDate);
        }

        private async void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var chartText = cbCategory.SelectedItem.ToString();

            switch (chartText)
            {
                case "Daily":
                    await ShowDayTopCategories();
                    break;
                case "Weekly":
                    await ShowWeekTopCategories();
                    break;
                case "Monthly":
                    await ShowMonthTopCategories();
                    break;
                case "Annually":
                    await ShowAnnualTopCategories();
                    break;
            }
        }

        private async Task ConfigEmployeeChart(DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var sales = await _salesService.GetSales(startDate, endDate);

                var employeeTransactions = sales
                    .GroupBy(sale => sale.Transaction.Employee.Id)
                    .Select(group => new
                    {
                        Id = group.Key,
                        EmployeeSales = group.Sum(t => t.SubTotal)
                    })
                    .OrderByDescending(emp => emp.EmployeeSales)
                    .ToList();

                var dataset = new GunaBarDataset();
                var employees = await _unitOfWork.EmployeeRepo.Get();

                foreach (var employee in employeeTransactions)
                {
                    var employeeName = employees.FirstOrDefault(e => e.Id == employee.Id)?.FullName;
                    dataset.DataPoints.Add(employeeName, (double) employee.EmployeeSales);
                }

                chartEmployee.Reset();
                chartEmployee.ApplyConfig(LightChartConfig.PieChartConfig(), Color.White);
                chartEmployee.Datasets.Add(dataset);
                chartEmployee.YAxes.GridLines.Display = false;
                chartEmployee.Legend.Display = false;
                chartEmployee.Update();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ParentForm, ex.Message, "An Error Occurred",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
            }
        }

        private async Task ShowDayTopEmployee()
        {
            var startDate = DateTime.Today.AddDays(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigEmployeeChart(startDate, endDate);
        }

        private async Task ShowWeekTopEmployee()
        {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigEmployeeChart(startDate, endDate);
        }

        private async Task ShowMonthTopEmployee()
        {
            var startDate = DateTime.Today.AddMonths(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigEmployeeChart(startDate, endDate);
        }

        private async Task ShowAnnualTopEmployee()
        {
            var startDate = DateTime.Today.AddYears(-1);
            var endDate = DateTime.Today.AddDays(1);

            await ConfigEmployeeChart(startDate, endDate);
        }

        private async void cbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            var chartText = cbEmployee.SelectedItem.ToString();

            switch (chartText)
            {
                case "Daily":
                    await ShowDayTopEmployee();
                    break;
                case "Weekly":
                    await ShowWeekTopEmployee();
                    break;
                case "Monthly":
                    await ShowMonthTopEmployee();
                    break;
                case "Annually":
                    await ShowAnnualTopEmployee();
                    break;
            }
        }
    }
}