using LaundryPOS.Contracts;
using LaundryPOS.Data;
using LaundryPOS.Models;
using LaundryPOS.Repositories;

namespace LaundryPOS.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        // Temporary
        private IEmployeeRepository _employeeRepo;
        private ITransactionRepository _transactionRepo;
        private ITransactionItemRepository _transactionItemRepo;
        private IItemRepository _itemRepo;
        private IAppSettingsRepository _appSettingsRepo;
        private IBaseRepository<Category> _categoryRepo;

        public IEmployeeRepository EmployeeRepo =>
            _employeeRepo ??= new EmployeeRepository(_context);

        public IItemRepository ItemRepo =>
            _itemRepo ??= new ItemRepository(_context);

        public ITransactionRepository TransactionRepo =>
            _transactionRepo ??= new TransactionRepository(_context);

        public IAppSettingsRepository AppSettingsRepo =>
            _appSettingsRepo ??= new AppSettingsRepository(_context);

        public IBaseRepository<Category> CategoryRepo =>
            _categoryRepo ??= new BaseRepository<Category>(_context);

        public ITransactionItemRepository TransactionItemRepo =>
            _transactionItemRepo ??= new TransactionItemRepository(_context);

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