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
        }

        private async Task InitializeSalesChart()
        {
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
                dataset.DataPoints.Add(formattedDate, (double?) salesForDate?.TotalSales ?? 0);
            }

            salesChart.Datasets.Add(dataset);
            salesChart.Update();
        }
    }
}