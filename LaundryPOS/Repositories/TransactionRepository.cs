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
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext options) : base(options)
        {

        }

        public async Task<int> GetLatestTransactionId()
        {
            var latestTransaction = await _context.Transactions.OrderByDescending(t => t.TransactionId)
                .FirstOrDefaultAsync();

            return latestTransaction?.TransactionId + 1 ?? 0;
        }
    }
}