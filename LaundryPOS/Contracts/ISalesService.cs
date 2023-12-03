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
        Task<List<ChartSale>> GetWeeklySalesChartData(DateTime startDate, DateTime endDate);
        Task<List<ChartSale>> GetMonthlySalesChartData(DateTime startDate, DateTime endDate);
        Task<List<ChartSale>> GetAnnualSalesChartData(DateTime startDate, DateTime endDate);
        Task<List<ChartSale>> GetDailySalesChartData(DateTime startDate, DateTime endDate);
        Task<List<ChartItem>> GetChartItemData(DateTime startDate, DateTime endDate);
        Task<List<ChartItem>> GetChartCategoryData(DateTime startDate, DateTime endDate);
        Task<List<ChartEmployee>> GetChartEmployeeData(DateTime startDate, DateTime endDate);
    }
}