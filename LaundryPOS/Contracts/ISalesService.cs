using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;

namespace LaundryPOS.Contracts
{
    public interface ISalesService
    {
        Task<decimal> GetSalesTotal(DateTime startDate, DateTime endDate);
        Task<decimal> GetAnnualSales();
        Task<decimal> GetMonthlySales();
        Task<decimal> GetWeeklySales();
        Task<decimal> GetDailySales();
        Task<IEnumerable<TransactionItem>> GetSales(DateTime startDate, DateTime endDate);
        Task<List<Sale>> GetWeeklySalesChartData(DateTime startDate, DateTime endDate);
        Task<List<Sale>> GetMonthlySalesChartData(DateTime startDate, DateTime endDate);
    }
}