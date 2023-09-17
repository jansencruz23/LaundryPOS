using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Contracts
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepo { get; }
        IBaseRepository<Item> ItemRepo { get; }
        IBaseRepository<AppSettings> AppSettingsRepo { get; }
        IBaseRepository<Transaction> TransactionRepo { get; }
        IBaseRepository<TransactionItem> TransactionItemRepo { get; }
        Task SaveAsync();
    }
}