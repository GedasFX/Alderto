using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services
{
    public interface IGuildLogger
    {


        /// <summary>
        /// Logs the transaction in the guild bank.
        /// </summary>
        /// <param name="bank">Bank in which transaction occured.</param>
        /// <param name="changedItem">Item that was changed. For Create, Update - Final state, for Delete - Previous state.</param>
        /// <param name="adminId">Id of member, who has administered the transaction.</param>
        /// <param name="transactorId">Id of member, who has transferred the goods.</param>
        /// <param name="comment">WithDescription field describing what happened in the transaction.</param>
        Task LogBankItemChangeAsync(GuildBank bank, GuildBankItem changedItem, ulong adminId, ulong transactorId,
            string comment);

        Task LogBankItemDeleteAsync(GuildBank bank, GuildBankItem deletedItem, ulong adminId);

        /// <summary>
        /// Logs the creation of a guild bank.
        /// </summary>
        /// <param name="adminId">Id of user who created the bank.</param>
        /// <param name="bank">The newly created bank.</param>
        Task LogBankCreateAsync(GuildBank bank, ulong adminId);

        /// <summary>
        /// Logs the changes of a guild bank.
        /// </summary>
        /// <param name="oldBank">Bank snapshot before changes were applied.</param>
        /// <param name="newBank">Bank snapshot after changes were applied.</param>
        /// <param name="adminId">Id of user who changed the bank.</param>
        Task LogBankUpdateAsync(GuildBank oldBank, GuildBank newBank, ulong adminId);

        /// <summary>
        /// Logs the removal of a guild bank.
        /// </summary>
        /// <param name="bank">Bank that was deleted.</param>
        /// <param name="adminId">Id of user who changed the bank.</param>
        Task LogBankDeleteAsync(GuildBank bank, ulong adminId);
    }
}