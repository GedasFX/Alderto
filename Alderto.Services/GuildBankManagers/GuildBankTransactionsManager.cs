using System;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services.GuildBankManagers
{
    public class GuildBankTransactionsManager : IGuildBankTransactionsManager
    {
        private readonly IAldertoDbContext _context;

        public GuildBankTransactionsManager(IAldertoDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(int bankId, ulong adminId, ulong transactorId, double amountDelta, int itemId = 0, string comment = null,
            bool saveChanges = false)
        {
            var message = new GuildBankTransaction
            {
                BankId = bankId,
                AdminId = adminId,
                MemberId = transactorId,
                Amount = amountDelta,
                ItemId = itemId != 0 ? (int?)itemId : null,
                Comment = comment,
                TransactionDate = DateTimeOffset.UtcNow
            };

            await _context.GuildBankTransactions.AddAsync(message);

            if (saveChanges)
                await _context.SaveChangesAsync();
        }
    }
}