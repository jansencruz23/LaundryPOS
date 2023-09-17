﻿using LaundryPOS.Contracts;
using LaundryPOS.Data;
using LaundryPOS.Models;
using LaundryPOS.Repositories;
using Microsoft.EntityFrameworkCore;
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
        private IBaseRepository<Item> _serviceRepo;
        private IBaseRepository<AppSettings> _appSettingsRepo;
        private IBaseRepository<Transaction> _transactionRepo;
        private IBaseRepository<TransactionItem> _transactionItemRepo;

        public IEmployeeRepository EmployeeRepo =>
            _employeeRepo ??= new EmployeeRepository(_context);

        public IBaseRepository<Item> ItemRepo =>
            _serviceRepo ??= new BaseRepository<Item>(_context);

        public IBaseRepository<AppSettings> AppSettingsRepo =>
            _appSettingsRepo ??= new BaseRepository<AppSettings>(_context);

        public IBaseRepository<Transaction> TransactionRepo =>
            _transactionRepo ??= new BaseRepository<Transaction>(_context);

        public IBaseRepository<TransactionItem> TransactionItemRepo =>
            _transactionItemRepo ??= new BaseRepository<TransactionItem>(_context);

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