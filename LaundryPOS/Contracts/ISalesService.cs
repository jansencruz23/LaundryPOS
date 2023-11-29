namespace LaundryPOS.Contracts
{
    public interface ISalesService
    {
        Task<decimal> GetSales(DateTime startDate, DateTime endDate);
        Task<decimal> GetAnnualSales();
        Task<decimal> GetMonthlySales();
        Task<decimal> GetWeeklySales();
        Task<decimal> GetDailySales();
    }
}