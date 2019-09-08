using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.GuildBankManagers
{
    public class GuildBankItemManager : IGuildBankItemManager
    {
        private readonly IAldertoDbContext _context;

        public GuildBankItemManager(IAldertoDbContext context)
        {
            _context = context;
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

        public async Task<GuildBankItem> CreateBankItemAsync(int bankId, GuildBankItem item)
        {
            item.GuildBankId = bankId;

            _context.GuildBankItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateBankItemAsync(int itemId, Action<GuildBankItem> changes)
        {
            var item = await GetBankItemAsync(itemId);
            changes(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBankItemAsync(int itemId)
        {
            _context.Remove(await GetBankItemAsync(itemId));
            await _context.SaveChangesAsync();
        }
    }
}
