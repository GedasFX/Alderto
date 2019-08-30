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
        
        public async Task<GuildBankItem> CreateItemAsync(ulong guildId, string name, string description = null, double value = 0, string imageUrl = null)
        {
            var item = new GuildBankItem
            {
                GuildId = guildId,
                Name = name,
                Description = description,
                Value = value,
                ImageUrl = imageUrl
            };

            _context.GuildBankItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateItemAsync(ulong guildId, string itemName, Action<GuildBankItem> changes)
        {
            var item = await GetItemAsync(guildId, itemName);

            changes(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(ulong guildId, string itemName)
        {
            var item = await GetItemAsync(guildId, itemName);
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        public Task<GuildBankItem> GetItemAsync(ulong guildId, string itemName)
        {
            return _context.GuildBankItems.SingleOrDefaultAsync(i => i.GuildId == guildId && i.Name == itemName);
        }

        public async Task<GuildBankBankItem> CreateBankItemAsync(int bankId, int itemId)
        {
            var gbItem = new GuildBankBankItem
            {
                GuildBankId = bankId,
                GuildBankItemId = itemId
            };
            await _context.GuildBankContents.AddAsync(gbItem);
            return gbItem;
        }

        public async Task UpdateBankItemAsync(int bankId, int itemId, Action<GuildBankBankItem> changes)
        {
            var item = await GetBankItemAsync(bankId, itemId);
            changes(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBankItemAsync(int bankId, int itemId)
        {
            _context.Remove(await GetBankItemAsync(bankId, itemId));
            await _context.SaveChangesAsync();
        }

        public Task<GuildBankBankItem> GetBankItemAsync(int bankId, int itemId)
        {
            return _context.GuildBankContents.FindAsync(bankId, itemId);
        }

        public Task<List<GuildBankItem>> GetGuildItems(ulong guildId)
        {
            return _context.GuildBankItems.Where(u => u.GuildId == guildId).ToListAsync();
        }

        public Task<List<GuildBankBankItem>> GetGuildBankItems(int bankId)
        {
            return _context.GuildBankContents.Where(u => u.GuildBankId == bankId).ToListAsync();
        }
    }
}
