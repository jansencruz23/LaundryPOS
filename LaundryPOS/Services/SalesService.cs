using LaundryPOS.Contracts;
using LaundryPOS.Helpers;
using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace LaundryPOS.Services
{
    public class SalesService : ISalesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public SalesService(IUnitOfWork unitOfWork,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<decimal> GetSalesTotal(DateTime startDate,
            DateTime endDate)
        {
            string cacheKey = $"SalesTotal_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var sales = await _unitOfWork.TransactionItemRepo
                    .Get(includeProperties: "Transaction,Item",
                        filter: ti => ti.Transaction.TransactionDate >= startDate
                                     && ti.Transaction.TransactionDate <= endDate
                                     && ti.Transaction.IsCompleted);

                decimal salesTotal = sales.Sum(sale => sale.SubTotal);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return salesTotal;
            });
        }

        public async Task<decimal> GetAnnualSales()
        {
            return await _cache.GetOrCreateAsync("AnnualSales", async entry =>
            {
                var startDate = new DateTime(DateTime.Today.Year, TimeConstants.FIRST_MONTH, TimeConstants.FIRST_DAY);
                var endDate = new DateTime(DateTime.Today.Year, TimeConstants.LAST_MONTH,
                    TimeConstants.LAST_DAY, TimeConstants.LAST_HOUR, TimeConstants.LAST_MINUTE,
                    TimeConstants.LAST_SECOND);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetSalesTotal(startDate, endDate);
            });
        }

        public async Task<decimal> GetMonthlySales()
        {
            return await _cache.GetOrCreateAsync("MonthlySales", async entry =>
            {
                var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var endDate = startDate.AddMonths(TimeConstants.NEXT_MONTH).AddTicks(-1);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetSalesTotal(startDate, endDate);
            });
        }

        public async Task<decimal> GetWeeklySales()
        {
            return await _cache.GetOrCreateAsync("WeeklySales", async entry =>
            {
                var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var endDate = startDate.AddDays(TimeConstants.DAYS_IN_WEEK);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetSalesTotal(startDate, endDate);
            });
        }

        public async Task<decimal> GetDailySales()
        {
            return await _cache.GetOrCreateAsync("DailySales", async entry =>
            {
                var startDate = DateTime.Today;
                var endDate = startDate.AddDays(TimeConstants.NEXT_DAY).AddTicks(-1);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetSalesTotal(startDate, endDate);
            });
        }

        public async Task<IEnumerable<TransactionItem>> GetSales(DateTime startDate,
            DateTime endDate)
        {
            string cacheKey = $"{startDate}-{endDate}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var sales = await _unitOfWork.TransactionItemRepo
                    .Get(includeProperties: "Transaction,Item,Transaction.Employee",
                        filter: ti => ti.Transaction.TransactionDate >= startDate
                            && ti.Transaction.TransactionDate <= endDate
                            && ti.Transaction.IsCompleted);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return sales;
            }) ?? Enumerable.Empty<TransactionItem>(); ;
        }

        public async Task<List<ChartSale>> GetSalesChartData(
            DateTime startDate, DateTime endDate,
            Func<Transaction, int> groupSelector)
        {
            string cacheKey = $"{startDate}-{endDate}-{groupSelector.Method.Name}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var sales = await GetSales(startDate, endDate);
                var salesChartData = sales
                    .GroupBy(ti => groupSelector(ti.Transaction))
                    .Select(group => new ChartSale
                    {
                        DateNumber = group.Key,
                        Sales = group.Sum(ti => ti.SubTotal)
                    })
                    .ToList();

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return salesChartData;
            }) ?? new List<ChartSale>();
        }

        public async Task<List<ChartSale>> GetWeeklySalesChartData(DateTime startDate, DateTime endDate)
        {
            string cacheKey = $"Weekly-{startDate}-{endDate}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var salesData = await GetSalesChartData(startDate, endDate, ti =>
                    CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                        ti.TransactionDate,
                        CalendarWeekRule.FirstFourDayWeek,
                        DayOfWeek.Sunday));

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return salesData;
            }) ?? new List<ChartSale>();
        }

        public async Task<List<ChartSale>> GetMonthlySalesChartData(DateTime startDate, DateTime endDate)
            => await GetSalesChartData(startDate, endDate, ti => ti.TransactionDate.Month);

        public async Task<List<ChartSale>> GetAnnualSalesChartData(DateTime startDate, DateTime endDate)
            => await GetSalesChartData(startDate, endDate, ti => ti.TransactionDate.Year);

        public async Task<List<ChartSale>> GetDailySalesChartData(DateTime startDate, DateTime endDate)
        {
            string cacheKey = $"DailySalesChartData_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var sales = await GetSales(startDate, endDate);
                var dailySalesChartData = sales
                    .GroupBy(ti => ti.Transaction.TransactionDate.Date)
                    .Select(group => new ChartSale
                    {
                        Date = group.Key.Date,
                        Sales = group.Sum(ti => ti.SubTotal)
                    })
                    .ToList();

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return dailySalesChartData;
            }) ?? new List<ChartSale>();
        }

        public async Task<List<ChartItem>> GetChartItemData(DateTime startDate, DateTime endDate)
        {
            string cacheKey = $"ChartItem-{startDate}-{endDate}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var sales = await GetSales(startDate, endDate);
                var chartItemData = sales
                    .GroupBy(item => item.ItemId)
                    .Select(group => new ChartItem
                    {
                        Id = group.Key,
                        Quantity = group.Sum(item => item.Quantity),
                    })
                    .OrderByDescending(item => item.Quantity)
                    .Take(10)
                    .ToList();

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return chartItemData;
            }) ?? new List<ChartItem>();
        }

        public async Task<List<ChartItem>> GetChartCategoryData(DateTime startDate, DateTime endDate)
        {
            string cacheKey = $"ChartCategory-{startDate}-{endDate}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var sales = await GetSales(startDate, endDate);
                var chartCategoryData = sales
                    .GroupBy(item => item.Item.CategoryId)
                    .Select(group => new ChartItem
                    {
                        Id = group.Key,
                        Quantity = group.Sum(item => item.Quantity),
                    })
                    .OrderByDescending(category => category.Quantity)
                    .ToList();

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return chartCategoryData;
            }) ?? new List<ChartItem>();
        }

        public async Task<List<ChartEmployee>> GetChartEmployeeData(DateTime startDate, DateTime endDate)
        {
            string cacheKey = $"ChartEmployee-{startDate}-{endDate}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var sales = await GetSales(startDate, endDate);
                var chartEmployeeData = sales
                    .GroupBy(sale => sale.Transaction.Employee.Id)
                    .Select(group => new ChartEmployee
                    {
                        Id = group.Key,
                        Sales = group.Sum(t => t.SubTotal)
                    })
                    .OrderByDescending(emp => emp.Sales)
                    .Take(10)
                    .ToList();

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return chartEmployeeData;
            }) ?? new List<ChartEmployee>();
        }
    }
}