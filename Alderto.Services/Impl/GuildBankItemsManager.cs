using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.Impl
{
    public class GuildBankItemsManager : IGuildBankItemsManager
    {
        private readonly AldertoDbContext _context;
        private readonly IGuildLogger _transactions;

        public GuildBankItemsManager(AldertoDbContext context, IGuildLogger transactions)
        {
            _context = context;
            _transactions = transactions;
        }

        public Task<GuildBankItem> GetBankItemAsync(int itemId, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>> options = null)
        {
            var items = _context.GuildBankItems as IQueryable<GuildBankItem>;
            if (options != null)
                items = options.Invoke(items);
            return items.SingleOrDefaultAsync(i => i.Id == itemId);
        }

        public Task<GuildBankItem> GetBankItemAsync(int bankId, string itemName, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>> options = null)
        {
            var items = _context.GuildBankItems as IQueryable<GuildBankItem>;
            if (options != null)
                items = options.Invoke(items);
            return items.SingleOrDefaultAsync(i => i.GuildBankId == bankId && i.Name == itemName);
        }

        public Task<List<GuildBankItem>> GetGuildBankContentsAsync(int bankId, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>> options = null)
        {
            var items = _context.GuildBankItems as IQueryable<GuildBankItem>;
            if (options != null)
                items = options.Invoke(items);
            return items.Where(u => u.GuildBankId == bankId).ToListAsync();
        }

        public async Task<GuildBankItem> CreateBankItemAsync(GuildBank bank, GuildBankItem item, ulong adminId)
        {
            // Ensure item is in the correct bank.
            item.GuildBankId = bank.Id;

            // Add the bank to database.
            _context.GuildBankItems.Add(item);

            // Log changes.
            await _transactions.LogBankItemCreateAsync(bank, item, adminId);

            // Save Changes
            await _context.SaveChangesAsync();

            // Lastly return the item.
            return item;
        }

        public async Task UpdateBankItemAsync(int itemId, ulong adminId, Action<GuildBankItem> changes, ulong? transactorId = null)
        {
            // Get the item.
            var item = await GetBankItemAsync(itemId, o => o.Include(i => i.GuildBank));

            // Create a copy for logging purposes.
            var oldItem = item.MemberwiseClone();

            // Apply changes.
            changes(item);

            // Ensure core values weren't changed.
            item.Id = oldItem.Id;
            item.GuildBankId = oldItem.GuildBankId;

            // Log changes.
            await _transactions.LogBankItemUpdateAsync(item.GuildBank, oldItem, item, adminId);

            // Save Changes
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBankItemQuantityAsync(int itemId, ulong adminId, double deltaQuantity, ulong? transactorId = null)
        {
            // Get the item.
            var item = await GetBankItemAsync(itemId, o => o.Include(i => i.GuildBank));

            // Apply quantity change.
            item.Quantity += deltaQuantity;

            // Log changes.
            await _transactions.LogBankItemQuantityUpdateAsync(item.GuildBank, item, deltaQuantity, adminId);

            // Save Changes
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBankItemAsync(int itemId, ulong moderatorId)
        {
            // Get the item.
            var item = await GetBankItemAsync(itemId, o => o.Include(i => i.GuildBank));

            // Remove from database.
            _context.Remove(item);

            // Log removal.
            await _transactions.LogBankItemDeleteAsync(item.GuildBank, item, moderatorId);

            await _context.SaveChangesAsync();
        }
    }
}
