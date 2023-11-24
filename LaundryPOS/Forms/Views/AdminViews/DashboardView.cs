using Guna.Charts.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Forms.Views.BaseViews;
using System.Globalization;

namespace LaundryPOS.Forms.Views.AdminViews
{
    public partial class DashboardView : BaseItemView
    {
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
            MessageBox.Show("" + await GetWeekSales());
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
            var startDate = new DateTime(DateTime.Today.Year, 1, 1);
            var endDate = new DateTime(DateTime.Today.Year, 12, 31, 12, 59, 59);

            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

            return sales.Sum(sale => sale.SubTotal);
        }

        private async Task<decimal> GetWeekSales()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endDate = startDate.AddDays(7);

            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

            return sales.Sum(sale => sale.SubTotal);
        }
    }
}