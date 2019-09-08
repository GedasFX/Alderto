using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services.GuildBankManagers
{
    public interface IGuildBankTransactionsManager
    {
        /// <summary>
        /// Logs the transaction in the guild bank.
        /// </summary>
        /// <param name="bank">Bank in which transaction occured.</param>
        /// <param name="adminId">Id of member, who has administered the transaction.</param>
        /// <param name="transactorId">Id of member, who has transferred the goods.</param>
        /// <param name="prevAmount">Amount of goods before the transaction started.</param>
        /// <param name="newAmount">Amount of goods after the transaction ended.</param>
        /// <param name="comment">Optional comment.</param>
        Task LogCurrencyChangeAsync(GuildBank bank, ulong adminId, ulong transactorId,
            double prevAmount, double newAmount, string comment = null);
    }
}