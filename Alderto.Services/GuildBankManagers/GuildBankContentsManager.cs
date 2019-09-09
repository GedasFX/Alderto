using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.GuildBankManagers
{
    public class GuildBankContentsManager : IGuildBankContentsManager
    {
        private readonly IAldertoDbContext _context;
        private readonly IGuildLogger _transactions;

        public GuildBankContentsManager(IAldertoDbContext context, IGuildLogger transactions)
        {
            _context = context;
            _transactions = transactions;
        }

        public Task<GuildBankItem> GetBankItemAsync(int itemId)
        {
            return _context.GuildBankItems.FindAsync(itemId);
        }

        public Task<GuildBankItem> GetBankItemAsync(int bankId, string itemName)
        {
            return _context.GuildBankItems.SingleOrDefaultAsync(i => i.GuildBankId == bankId && i.Name == itemName);
        }

        public Task<List<GuildBankItem>> GetGuildBankContentsAsync(int bankId)
        {
            return _context.GuildBankItems.Where(u => u.GuildBankId == bankId).ToListAsync();
        }

        public async Task<GuildBankItem> CreateBankItemAsync(GuildBank bank, GuildBankItem item, ulong adminId)
        {
            item.GuildBankId = bank.Id;

            _context.GuildBankItems.Add(item);
            await _context.SaveChangesAsync();
            await _transactions.LogBankItemChangeAsync(bank, item, adminId, adminId, $"{item.Quantity} **{item.Name}** added to the bank.");
            return item;
        }

        public async Task UpdateBankItemAsync(int itemId, ulong adminId, Action<GuildBankItem> changes, ulong? transactorId = null)
        {
            var item = await GetBankItemAsync(itemId);
            changes(item);
            await _context.SaveChangesAsync();
        }

        public Task UpdateBankItemQuantityAsync(int itemId, ulong adminId, double deltaQuantity, ulong? transactorId = null)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveBankItemAsync(int itemId, ulong moderatorId)
        {
            _context.Remove(await GetBankItemAsync(itemId));
            await _context.SaveChangesAsync();
        }
    }
}
