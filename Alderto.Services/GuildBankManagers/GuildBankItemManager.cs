using System;
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

        public async Task CreateAsync(ulong guildId, GuildBankItem item)
        {
            item.GuildId = guildId;

            _context.GuildBankItems.Add(item);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateAsync(ulong guildId, string itemName, Action<GuildBankItem> changes)
        {
            var item = await GetItemAsync(guildId, itemName);

            changes(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ulong guildId, string itemName)
        {
            var item = await GetItemAsync(guildId, itemName);
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<GuildBankItem> GetItemAsync(ulong guildId, string itemName)
        {
            return await _context.GuildBankItems.SingleOrDefaultAsync(i => i.GuildId == guildId && i.Name == itemName);
        }

        public GuildBankBankItem GetBankItem(GuildBank bank, string itemName)
        {

            return bank.GuildBankContents.SingleOrDefault(i => i.GuildBankItem.Name == itemName);
        }
    }
}
