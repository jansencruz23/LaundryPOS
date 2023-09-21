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
    public class TransactionItemRepository : BaseRepository<TransactionItem>, ITransactionItemRepository 
    {
        public TransactionItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TransactionItem>> GetTransactionItems(int transactionId)
        {
            return await _context.TransactionItems
                .Include(ti => ti.Transaction)
                    .ThenInclude(t => t.Employee)
                .Include(ti => ti.Item)
                .Where(ti => ti.TransactionId == transactionId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}