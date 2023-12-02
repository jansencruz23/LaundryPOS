using LaundryPOS.Models;

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
    }
}