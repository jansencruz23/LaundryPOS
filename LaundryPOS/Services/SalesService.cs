﻿using LaundryPOS.Contracts;
using LaundryPOS.Helpers;
using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;
using System.Globalization;

namespace LaundryPOS.Services
{
    public class SalesService : ISalesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> GetSalesTotal(DateTime startDate,
            DateTime endDate)
        {
            var sales = await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item",
                filter: ti => ti.Transaction.TransactionDate >= startDate
                    && ti.Transaction.TransactionDate <= endDate
                    && ti.Transaction.IsCompleted);

            return sales.Sum(sale => sale.SubTotal);
        }

        public async Task<decimal> GetAnnualSales()
        {
            var startDate = new DateTime(DateTime.Today.Year, TimeConstants.FIRST_MONTH, TimeConstants.FIRST_DAY);
            var endDate = new DateTime(DateTime.Today.Year, TimeConstants.LAST_MONTH,
                TimeConstants.LAST_DAY, TimeConstants.LAST_HOUR, TimeConstants.LAST_MINUTE,
                TimeConstants.LAST_SECOND);

            return await GetSalesTotal(startDate, endDate);
        }

        public async Task<decimal> GetMonthlySales()
        {
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endDate = startDate.AddMonths(TimeConstants.NEXT_MONTH).AddTicks(-1);

            return await GetSalesTotal(startDate, endDate);
        }

        public async Task<decimal> GetWeeklySales()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var endDate = startDate.AddDays(TimeConstants.DAYS_IN_WEEK);

            return await GetSalesTotal(startDate, endDate);
        }

        public async Task<decimal> GetDailySales()
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(TimeConstants.NEXT_DAY).AddTicks(-1);

            return await GetSalesTotal(startDate, endDate);
        }

        public async Task<IEnumerable<TransactionItem>> GetSales(DateTime startDate,
            DateTime endDate)
            => await _unitOfWork.TransactionItemRepo
                .Get(includeProperties: "Transaction,Item,Transaction.Employee",
                    filter: ti => ti.Transaction.TransactionDate >= startDate
                        && ti.Transaction.TransactionDate <= endDate
                        && ti.Transaction.IsCompleted);

        public async Task<List<Sale>> GetSalesChartData(
            DateTime startDate, DateTime endDate,
            Func<Transaction, int> groupSelector)
        {
            var sales = await GetSales(startDate, endDate);
            return sales
                .GroupBy(ti => groupSelector(ti.Transaction))
                .Select(group => new Sale
                {
                    DateNumber = group.Key,
                    Sales = group.Sum(ti => ti.SubTotal)
                })
                .ToList();
        }

        public async Task<List<Sale>> GetWeeklySalesChartData(DateTime startDate, DateTime endDate)
        {
            return await GetSalesChartData(startDate, endDate, ti =>
                CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    ti.TransactionDate,
                    CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Sunday));
        }

        public async Task<List<Sale>> GetMonthlySalesChartData(DateTime startDate, DateTime endDate)
        {
            return await GetSalesChartData(startDate, endDate, ti => ti.TransactionDate.Month);
        }

        public async Task<List<Sale>> GetAnnualSalesChartData(DateTime startDate, DateTime endDate)
        {
            return await GetSalesChartData(startDate, endDate, ti => ti.TransactionDate.Year);
        }

        public async Task<List<Sale>> GetDailySalesChartData(DateTime startDate, DateTime endDate)
        {
            var sales = await GetSales(startDate, endDate);
            return sales
                .GroupBy(ti => ti.Transaction.TransactionDate.Date)
                .Select(group => new Sale
                {
                    Date = group.Key.Date,
                    Sales = group.Sum(ti => ti.SubTotal)
                })
               .ToList();
        }
    }
}