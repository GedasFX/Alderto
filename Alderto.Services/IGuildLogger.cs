using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services
{
    public interface IGuildLogger
    {
        /// <summary>
        /// Logs the creation of a bank item.
        /// </summary>
        /// <param name="bank">Bank in which transaction occured.</param>
        /// <param name="item">Item which was created.</param>
        /// <param name="modId">Id of user, who created the item.</param>
        Task LogBankItemCreateAsync(GuildBank bank, GuildBankItem item, ulong modId);

        /// <summary>
        /// Logs the transaction in the guild bank.
        /// </summary>
        /// <param name="bank">Bank in which transaction occured.</param>
        /// <param name="oldItem">Item snapshot before changes were applied.</param>
        /// <param name="newItem">Item snapshot after changes were applied.</param>
        /// <param name="modId">Id of member, who has administered the transaction.</param>
        Task LogBankItemUpdateAsync(GuildBank bank, GuildBankItem oldItem, GuildBankItem newItem, ulong modId);

        /// <summary>
        /// Logs the transaction in the guild bank.
        /// </summary>
        /// <param name="bank">Bank in which transaction occured.</param>
        /// <param name="item">Item of which quantity was modified.</param>
        /// <param name="amount">Amount the quantity was modified by.</param>
        /// <param name="modId">Id of user, who administrated the transaction.</param>
        /// <param name="transactorId">Id of user, who initiated the transaction. If null, <see cref="modId"/> is used.</param>
        Task LogBankItemQuantityUpdateAsync(GuildBank bank, GuildBankItem item, double amount, ulong modId,
            ulong? transactorId = null);

        /// <summary>
        /// Logs the deletion of a bank item.
        /// </summary>
        /// <param name="bank">Bank in which transaction occured.</param>
        /// <param name="item">Item which was deleted.</param>
        /// <param name="modId">Id of user, who deleted the item.</param>
        Task LogBankItemDeleteAsync(GuildBank bank, GuildBankItem item, ulong modId);

        /// <summary>
        /// Logs the creation of a guild bank.
        /// </summary>
        /// <param name="modId">Id of user who created the bank.</param>
        /// <param name="bank">The newly created bank.</param>
        Task LogBankCreateAsync(GuildBank bank, ulong modId);

        /// <summary>
        /// Logs the changes of a guild bank.
        /// </summary>
        /// <param name="oldBank">Bank snapshot before changes were applied.</param>
        /// <param name="newBank">Bank snapshot after changes were applied.</param>
        /// <param name="modId">Id of user who changed the bank.</param>
        Task LogBankUpdateAsync(GuildBank oldBank, GuildBank newBank, ulong modId);

        /// <summary>
        /// Logs the removal of a guild bank.
        /// </summary>
        /// <param name="bank">Bank that was deleted.</param>
        /// <param name="modId">Id of user who changed the bank.</param>
        Task LogBankDeleteAsync(GuildBank bank, ulong modId);
    }
}