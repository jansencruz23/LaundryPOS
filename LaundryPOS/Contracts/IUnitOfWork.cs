﻿using LaundryPOS.Models;
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
        ITransactionItemRepository TransactionItemRepo { get; }
        IItemRepository ItemRepo { get; }
        ITransactionRepository TransactionRepo { get; }
        IAppSettingsRepository AppSettingsRepo { get; }
        IBaseRepository<Category> CategoryRepo { get; }
        Task SaveAsync();
    }
}