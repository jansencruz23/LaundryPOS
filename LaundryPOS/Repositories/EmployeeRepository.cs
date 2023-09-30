using LaundryPOS.Contracts;
using LaundryPOS.Data;
using LaundryPOS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Employee> GetEmployeeByUsername(string username)
        {
            return await _context.Employees
                .AsNoTracking()
                .Where(e => e.IsActive)
                .SingleOrDefaultAsync(e => e.Username.Equals(username));
        }

        public async Task<IEnumerable<Employee>> GetTransactedEmployee(int transactionId)
        {
            return await _context.Transactions
                .Where(t => t.TransactionId == transactionId)
                .Select(t => t.Employee)
                .ToListAsync();
        }

        public async Task<Employee> GetAdmin()
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.UserRole.Equals("admin"));
        }
    }
}