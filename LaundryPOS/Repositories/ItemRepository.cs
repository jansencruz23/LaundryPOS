using LaundryPOS.Contracts;
using LaundryPOS.Data;
using LaundryPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace LaundryPOS.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Item>> GetBoughtItems(int transactionId)
        {
            return await _context.TransactionItems
                .Where(ti => ti.TransactionId == transactionId)
                .Select(ti => ti.Item)
                .ToListAsync();
        }

        public void MarkAsModified(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}