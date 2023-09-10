using LaundryPOS.Contracts;
using LaundryPOS.Data;
using LaundryPOS.Models;
using LaundryPOS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        // Temporary
        private IBaseRepository<Employee> _employeeRepo;

        public IBaseRepository<Employee> EmployeeRepo =>
            _employeeRepo ??= new BaseRepository<Employee>(_context);

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}