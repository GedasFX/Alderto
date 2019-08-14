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
        private readonly IGuildBankTransactionsManager _transactions;
        private readonly IGuildBankItemManager _items;

        public GuildBankManager(IAldertoDbContext context, IGuildBankTransactionsManager transactions, IGuildBankItemManager items)
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

        public IEnumerable<GuildBankTransaction> GetAllTransactions(ulong guildId, ulong memberId,
            Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            var query = _context.GuildBanks.Include(b => b.Transactions) as IQueryable<GuildBank>;
            if (options != null)
                query = options(query);
            return query.Where(g => g.GuildId == guildId).SelectMany(g => g.Transactions.Where(t => t.MemberId == memberId));
        }

        public Task<GuildBank> GetGuildBankAsync(ulong guildId, string name, Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            return FetchGuildBanks(guildId, options).SingleOrDefaultAsync(b => b.Name == name);
        }

        public Task<GuildBank> GetGuildBankAsync(ulong guildId, int id, Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            return FetchGuildBanks(guildId, options).SingleOrDefaultAsync(b => b.Id == id);
        }

        public Task<List<GuildBank>> GetAllGuildBanks(ulong guildId, Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null)
        {
            return FetchGuildBanks(guildId, options).ToListAsync();
        }

        public async Task ModifyCurrencyCountAsync(ulong guildId, string bankName, ulong adminId, ulong transactorId, double quantity, string comment = null)
        {
            var bank = await GetGuildBankAsync(guildId, bankName);
            bank.CurrencyCount += quantity;

            await _transactions.LogAsync(bank.Id, adminId, transactorId, quantity, comment: comment);
            await _context.SaveChangesAsync();
        }

        public async Task ModifyItemCountAsync(ulong guildId, string bankName, ulong adminId, ulong transactorId, string itemName, double quantity, string comment = null)
        {
            var bank = await GetGuildBankAsync(guildId, bankName);
            if (bank == null)
                throw new NotImplementedException();

            var item = await _items.GetItemAsync(guildId, itemName);
            if (item == null)
                throw new NotImplementedException();

            var bankItem = await _items.GetBankItemAsync(bank.Id, item.Id) ??
                           await _items.CreateBankItemAsync(bank.Id, item.Id);

            bankItem.Quantity += quantity;

            await _transactions.LogAsync(bank.Id, adminId, transactorId, quantity, bankItem.GuildBankItemId, comment);
            await _context.SaveChangesAsync();
        }

        public async Task<GuildBank> CreateGuildBankAsync(ulong guildId, string name)
        {
            // If guild id is unspecified do nothing.
            if (guildId < 1)
                return null;

            // Ensure foreign key constraint is not violated.
            var guild = await _context.Guilds.FindAsync(guildId);
            if (guild == null)
            {
                guild = new Guild(guildId);
                await _context.Guilds.AddAsync(guild);
            }

            // Add the bank
            var bank = new GuildBank(guildId, name);
            await _context.GuildBanks.AddAsync(bank);

            // Save changes
            await _context.SaveChangesAsync();

            // Return the added bank
            return bank;
        }

        public async Task RemoveGuildBankAsync(ulong guildId, string name)
        {
            _context.GuildBanks.Remove(await GetGuildBankAsync(guildId, name));
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGuildBankAsync(ulong guildId, int id)
        {
            _context.GuildBanks.Remove(await GetGuildBankAsync(guildId, id));
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
    }
}