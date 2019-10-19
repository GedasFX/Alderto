using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.Impl
{
    public class GuildBankManager : IGuildBankManager
    {
        private readonly AldertoDbContext _context;
        private readonly IGuildLogger _transactions;

        public GuildBankManager(AldertoDbContext context, IGuildLogger transactions)
        {
            _context = context;
            _transactions = transactions;
        }

        private IQueryable<GuildBank> FetchGuildBanks(ulong guildId, Func<IQueryable<GuildBank>, IQueryable<GuildBank>>? options = null)
        {
            var query = _context.GuildBanks as IQueryable<GuildBank>;
            if (options != null)
                query = options.Invoke(query);
            return query.Where(b => b.GuildId == guildId);
        }

        public async Task<GuildBank?> GetGuildBankAsync(ulong guildId, string name, Func<IQueryable<GuildBank>, IQueryable<GuildBank>>? options = null)
        {
            return await FetchGuildBanks(guildId, options).SingleOrDefaultAsync(b => b.Name == name);
        }
        public async Task<GuildBank?> GetGuildBankAsync(ulong guildId, int id, Func<IQueryable<GuildBank>, IQueryable<GuildBank>>? options = null)
        {
            return await FetchGuildBanks(guildId, options).SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<GuildBank>> GetGuildBanksAsync(ulong guildId, Func<IQueryable<GuildBank>, IQueryable<GuildBank>>? options = null)
        {
            return await FetchGuildBanks(guildId, options).ToListAsync();
        }

        public async Task<GuildBank> CreateGuildBankAsync(ulong guildId, ulong adminId, GuildBank bank)
        {
            if (string.IsNullOrWhiteSpace(bank.Name))
                throw new NameNotProvidedException();

            // Ensure foreign key constraint is not violated.
            var guild = await _context.Guilds.FindAsync(guildId);
            if (guild == null)
            {
                guild = new Guild(guildId);
                _context.Guilds.Add(guild);
            }

            // Ensure the added bank's guild id is correct.
            bank.GuildId = guildId;

            // Add the bank
            _context.GuildBanks.Add(bank);

            // Log the creation of the bank
            await _transactions.LogBankCreateAsync(bank, adminId);

            // Save changes
            await _context.SaveChangesAsync();

            // Return the added bank
            return bank;
        }

        public async Task UpdateGuildBankAsync(ulong guildId, string name, ulong adminId, Action<GuildBank> changes)
        {
            await UpdateGuildBankAsync(await GetGuildBankAsync(guildId, name), adminId, changes);
        }
        public async Task UpdateGuildBankAsync(ulong guildId, int id, ulong adminId, Action<GuildBank> changes)
        {
            await UpdateGuildBankAsync(await GetGuildBankAsync(guildId, id), adminId, changes);
        }

        private async Task UpdateGuildBankAsync(GuildBank? bank, ulong adminId, Action<GuildBank> changes)
        {
            if (bank == null)
                throw new BankNotFoundException();

            // Take a snapshot of the bank pre changes.
            var initialBank = bank.MemberwiseClone();

            // Apply the changes.
            changes(bank);

            // Ensure that core keys are intact.
            bank.Id = initialBank.Id;
            bank.GuildId = initialBank.GuildId;

            // Log the modification of the bank
            await _transactions.LogBankUpdateAsync(initialBank, bank, adminId);

            // Save changes
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGuildBankAsync(ulong guildId, string name, ulong adminId)
        {
            await RemoveGuildBankAsync(await GetGuildBankAsync(guildId, name, b => b.Include(e => e.Contents)), adminId);
        }
        public async Task RemoveGuildBankAsync(ulong guildId, int id, ulong adminId)
        {
            await RemoveGuildBankAsync(await GetGuildBankAsync(guildId, id, b => b.Include(e => e.Contents)), adminId);
        }
        private async Task RemoveGuildBankAsync(GuildBank? bank, ulong adminId)
        {
            if (bank == null)
                throw new BankNotFoundException();

            _context.GuildBanks.Remove(bank);
            await _transactions.LogBankDeleteAsync(bank, adminId);
            await _context.SaveChangesAsync();
        }
    }
}