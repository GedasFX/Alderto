using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;

namespace Alderto.Services.GuildBankManagers
{
    public interface IGuildBankContentsManager
    {
        /// <summary>
        /// Gets the bank item with from the given primary key.
        /// </summary>
        /// <param name="itemId">Primary key of the item.</param>
        /// <param name="options">Additional includes.</param>
        /// <returns>Item represented by the primary key.</returns>
        Task<GuildBankItem> GetBankItemAsync(int itemId, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>> options = null);

        /// <summary>
        /// Gets the bank item with from the given bank with the specified name.
        /// </summary>
        /// <param name="bankId">Primary key of the bank.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="options">Additional includes.</param>
        Task<GuildBankItem> GetBankItemAsync(int bankId, string itemName, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>> options = null);

        /// <summary>
        /// Fetches contents of the bank.
        /// </summary>
        /// <param name="bankId">Id of bank to get contents of.</param>
        /// <param name="options">Additional includes.</param>
        /// <returns>Bank contents.</returns>
        Task<List<GuildBankItem>> GetGuildBankContentsAsync(int bankId, Func<IQueryable<GuildBankItem>, IQueryable<GuildBankItem>> options = null);

        /// <summary>
        /// Creates a new item in a given bank.
        /// </summary>
        /// <param name="bank">Bank to add item to.</param>
        /// <param name="item">Item to add to the bank.</param>
        /// <param name="adminId">Id of user, who created the item.</param>
        /// <returns>Added item to the bank.</returns>
        Task<GuildBankItem> CreateBankItemAsync(GuildBank bank, GuildBankItem item, ulong adminId);

        /// <summary>
        /// Updates an existing item in the bank.
        /// </summary>
        /// <param name="itemId">Id of item to update.</param>
        /// <param name="adminId">Id of user, who administered the transaction.</param>
        /// <param name="transactorId">Id of user, who initiated the transaction.</param>
        /// <param name="changes">Changes to apply to the item.</param>
        Task UpdateBankItemAsync(int itemId, ulong adminId, Action<GuildBankItem> changes, ulong? transactorId = null);

        /// <summary>
        /// Updates the quantity of an item.
        /// </summary>
        /// <param name="itemId">Id of item to update.</param>
        /// <param name="adminId">Id of user, who administered the transaction.</param>
        /// <param name="deltaQuantity">Item amount changed.</param>
        /// <param name="transactorId">Id of user, who initiated the transaction. Defaults to <see cref="adminId"/></param>
        Task UpdateBankItemQuantityAsync(int itemId, ulong adminId, double deltaQuantity, ulong? transactorId = null);

        /// <summary>
        /// Removes an item from the bank.
        /// </summary>
        /// <param name="itemId">Id of item to remove.</param>
        /// <param name="moderatorId">Id of user, who removed the item.</param>
        Task RemoveBankItemAsync(int itemId, ulong moderatorId);
    }
}
