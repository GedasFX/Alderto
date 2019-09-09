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

        /// <summary>
        /// Logs the creation of a guild bank.
        /// </summary>
        /// <param name="guildId">Id of guild the bank exists in.</param>
        /// <param name="adminId">Id of user who created the bank.</param>
        /// <param name="newBank">The newly created bank.</param>
        Task LogBankCreateAsync(ulong guildId, ulong adminId, GuildBank newBank);

        /// <summary>
        /// Logs the changes of a guild bank.
        /// </summary>
        /// <param name="guildId">Id of guild the bank exists in.</param>
        /// <param name="adminId">Id of user who changed the bank.</param>
        /// <param name="oldBank">Bank snapshot before changes were applied.</param>
        /// <param name="newBank">Bank snapshot after changes were applied.</param>
        Task LogBankUpdateAsync(ulong guildId, ulong adminId, GuildBank oldBank, GuildBank newBank);
    }
}