using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.GuildBankManagers
{
    public class GuildBankManager : IGuildBankManager
    {
        private readonly IAldertoDbContext _context;
        private readonly IGuildLogger _transactions;
        private readonly IGuildBankContentsManager _items;

        public GuildBankManager(IAldertoDbContext context, IGuildLogger transactions, IGuildBankContentsManager items)
        {
            _context = context;
            _transactions = transactions;
            _items = items;
        }

        private IQueryable<GuildBank> FetchGuildBanks(ulong guildId, Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            var query = _context.GuildBanks as IQueryable<GuildBank>;
            if (options != null)
                query = options.Invoke(query);
            return query.Where(b => b.GuildId == guildId);
        }

        public Task<GuildBank> GetGuildBankAsync(ulong guildId, string name, Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            return FetchGuildBanks(guildId, options).SingleOrDefaultAsync(b => b.Name == name);
        }
        public Task<GuildBank> GetGuildBankAsync(ulong guildId, int id, Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            return FetchGuildBanks(guildId, options).SingleOrDefaultAsync(b => b.Id == id);
        }

        public Task<List<GuildBank>> GetGuildBanksAsync(ulong guildId, Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            return FetchGuildBanks(guildId, options).ToListAsync();
        }

        public async Task<GuildBank> CreateGuildBankAsync(ulong guildId, ulong adminId, string bankName, ulong? logChannelId = null)
        {
            // Ensure foreign key constraint is not violated.
            var guild = await _context.Guilds.FindAsync(guildId);
            if (guild == null)
            {
                guild = new Guild(guildId);
                _context.Guilds.Add(guild);
            }

            // Add the bank
            var bank = new GuildBank(guildId, bankName) { LogChannelId = logChannelId };
            _context.GuildBanks.Add(bank);

            // Save changes
            await _context.SaveChangesAsync();

            // Log the creation of the bank
            await _transactions.LogBankCreateAsync(guildId, adminId, bank);

            // Return the added bank
            return bank;
        }

        public async Task UpdateGuildBankAsync(ulong guildId, string name, Action<GuildBank> changes)
        {
            var gb = await GetGuildBankAsync(guildId, name);
            changes(gb);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateGuildBankAsync(ulong guildId, int id, Action<GuildBank> changes)
        {
            var gb = await GetGuildBankAsync(guildId, id);
            changes(gb);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGuildBankAsync(ulong guildId, string name)
        {
            _context.GuildBanks.Remove(await GetGuildBankAsync(guildId, name));
            await _context.SaveChangesAsync();
        }
        public async Task RemoveGuildBankAsync(ulong guildId, int id)
        {
            _context.GuildBanks.Remove(await GetGuildBankAsync(guildId, id));
            await _context.SaveChangesAsync();
        }
    }
}