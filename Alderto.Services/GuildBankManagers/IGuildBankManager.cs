using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services.GuildBankManagers
{
    public interface IGuildBankManager
    {
        /// <summary>
        /// Gets the member's transactions from the given banks.
        /// </summary>
        /// <param name="memberId">Member Id.</param>
        /// <param name="options">Additional includes.</param>
        /// <returns>A collection of transactions made by the user in the given guild bank.</returns>
        IEnumerable<GuildBankTransaction> GetAllTransactions(ulong memberId,
            Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null);

        /// <summary>
        /// Gets the guild bank with the provided name.
        /// </summary>
        /// <param name="name">Name of the bank.</param>
        /// <param name="options">Additional includes.</param>
        /// <returns>Guild bank with that name.</returns>
        Task<GuildBank> GetGuildBankAsync(string name,
            Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null);

        /// <summary>
        /// Gets all banks of the configured guild.
        /// </summary>
        /// <param name="options">Additional includes.</param>
        /// <returns>A collection of guild banks belonging to the given guild.</returns>
        IEnumerable<GuildBank> GetAllGuildBanks(Func<IQueryable<GuildBank>, IQueryable<GuildBank>> options = null);

        /// <summary>
        /// Modifies the amount of currency the guild has.
        /// </summary>
        /// <param name="bankName">Name of the bank the transaction has occured in.</param>
        /// <param name="transactorId">Id of person, who has given/taken money from/to the guild.</param>
        /// <param name="quantity">Amount of currency transferred.</param>
        /// <param name="comment">Comment on the transaction.</param>
        Task ModifyCurrencyCountAsync(string bankName, ulong transactorId, double quantity, string comment = null);


        /// <summary>
        /// Modifies the amount of a particular item the guild has.
        /// </summary>
        /// <param name="bankName">Name of the bank the transaction has occured in.</param>
        /// <param name="transactorId">Id of person, who has given/taken items from/to the guild.</param>
        /// <param name="itemName">Name of the item that was transferred.</param>
        /// <param name="quantity">Amount of items transferred.</param>
        /// <param name="comment">Comment on the transaction.</param>
        Task ModifyItemCountAsync(string bankName, ulong transactorId, string itemName, double quantity,
            string comment = null);

        /// <summary>
        /// Configures the <see cref="IGuildBankManager"/> environment.
        /// </summary>
        /// <param name="guildId">Id of guild the command was executed in.</param>
        /// <param name="adminUserId">Id of user, who is accessing the bank.</param>
        IGuildBankManager Configure(ulong guildId, ulong adminUserId);
    }
}