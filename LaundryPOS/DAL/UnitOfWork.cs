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
        private IEmployeeRepository _employeeRepo;
        private IBaseRepository<Service> _serviceRepo;

        public IEmployeeRepository EmployeeRepo =>
            _employeeRepo ??= new EmployeeRepository(_context);

        public IBaseRepository<Service> ServiceRepo =>
            _serviceRepo ??= new BaseRepository<Service>(_context);

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}