using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Contracts
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<bool> IsEmployeeExisting(string username);
        Task<Employee> GetEmployeeByUsername(string username);
        Task<IEnumerable<Employee>> GetTransactedEmployee(int transactionId);
        Task<Employee> GetAdmin();
    }
}