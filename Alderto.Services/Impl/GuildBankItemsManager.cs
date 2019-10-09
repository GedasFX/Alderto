using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.Exceptions.NotFound;
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

        public async Task<GuildBankItem?> GetBankItemAsync(GuildBank bank, int itemId, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>>? options = null)
        {
            var items = _context.GuildBankItems as IQueryable<GuildBankItem>;
            if (options != null)
                items = options.Invoke(items);
            return await items.SingleOrDefaultAsync(i => i.GuildBankId == bank.Id && i.Id == itemId);
        }

        public async Task<GuildBankItem?> GetBankItemAsync(GuildBank bank, string itemName, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>>? options = null)
        {
            var items = _context.GuildBankItems as IQueryable<GuildBankItem>;
            if (options != null)
                items = options.Invoke(items);
            return await items.SingleOrDefaultAsync(i => i.GuildBankId == bank.Id && i.Name == itemName);
        }

        public Task<List<GuildBankItem>> GetBankItemsAsync(GuildBank bank, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>>? options = null)
        {
            var items = _context.GuildBankItems as IQueryable<GuildBankItem>;
            if (options != null)
                items = options.Invoke(items);
            return items.Where(u => u.GuildBankId == bank.Id).ToListAsync();
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

        public async Task UpdateBankItemAsync(GuildBank bank, int itemId, ulong adminId, Action<GuildBankItem> changes, ulong? transactorId = null)
        {
            // Get the item.
            var item = await GetBankItemAsync(bank, itemId);

            // Check if item exists and is part of the bank.
            if (item == null || item.GuildBankId != bank.Id)
                throw new BankItemNotFoundException();

            // Create a copy for logging purposes.
            var oldItem = item.MemberwiseClone();

            // Apply changes.
            changes(item);

            // Ensure core values weren't changed.
            item.Id = oldItem.Id;
            item.GuildBankId = oldItem.GuildBankId;

            // Log changes.
            await _transactions.LogBankItemUpdateAsync(bank, oldItem, item, adminId);

            // Save Changes
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBankItemQuantityAsync(GuildBank bank, int itemId, ulong adminId, double deltaQuantity,
            ulong? transactorId = null)
        {
            // Get the item.
            var item = await GetBankItemAsync(bank, itemId);

            // Check if item exists and is part of the bank.
            if (item == null || item.GuildBankId != bank.Id)
                throw new BankItemNotFoundException();

            // Apply quantity change.
            item.Quantity += deltaQuantity;

            // Log changes.
            await _transactions.LogBankItemQuantityUpdateAsync(bank, item, deltaQuantity, adminId);

            // Save Changes
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBankItemAsync(GuildBank bank, int itemId, ulong moderatorId)
        {
            // Get the item.
            var item = await GetBankItemAsync(bank, itemId);

            // Check if item exists and is part of the bank.
            if (item == null || item.GuildBankId != bank.Id)
                throw new BankItemNotFoundException();

            // Remove from database.
            _context.Remove(item);

            // Log removal.
            await _transactions.LogBankItemDeleteAsync(bank, item, moderatorId);

            await _context.SaveChangesAsync();
        }
    }
}
