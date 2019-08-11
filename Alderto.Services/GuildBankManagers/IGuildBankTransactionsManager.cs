using System.Threading.Tasks;

namespace Alderto.Services.GuildBankManagers
{
    public interface IGuildBankTransactionsManager
    {
        /// <summary>
        /// Logs the transaction in the guild bank.
        /// </summary>
        /// <param name="bankId">Id of bank in which transaction occured.</param>
        /// <param name="adminId">Id of member, who has administered the transaction.</param>
        /// <param name="transactorId">Id of member, who has transferred the goods.</param>
        /// <param name="itemId">Id of good transferred. Use 0 if good was currency.</param>
        /// <param name="amountDelta">Amount of goods transferred.</param>
        /// <param name="comment">Optional comment.</param>
        /// <param name="saveChanges">Save changes in context.</param>
        Task LogAsync(int bankId, ulong adminId, ulong transactorId, double amountDelta, int itemId = 0, string comment = null, bool saveChanges = false);
    }
}