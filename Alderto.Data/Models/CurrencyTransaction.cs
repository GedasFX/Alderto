using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data.Models
{
    public class CurrencyTransaction
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Date of transaction.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Id of currency, which was sent.
        /// </summary>
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// Id of sender, who sent the currency.
        /// </summary>
        public ulong SenderId { get; set; }

        /// <summary>
        /// Id of recipient, who received the currency.
        /// </summary>
        public ulong RecipientId { get; set; }

        /// <summary>
        /// Amount of money that was transferred.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Is award. Means that the sender did not lose money in this transaction.
        /// </summary>
        public bool IsAward { get; set; }

        [ForeignKey(nameof(SenderId))]
        public virtual Member? Sender { get; set; }

        [ForeignKey(nameof(RecipientId))]
        public virtual Member? Recipient { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency? Currency { get; set; }

        public CurrencyTransaction()
        {
        }

        public CurrencyTransaction(Guid currencyId, ulong senderId, ulong recipientId, int amount,
            bool isAward = false)
        {
            CurrencyId = currencyId;
            SenderId = senderId;
            RecipientId = recipientId;
            Amount = amount;
            IsAward = isAward;
        }
    }

    public static class CurrencyTransactionRepository
    {
        public static IQueryable<CurrencyTransaction> ListItems(this IQueryable<CurrencyTransaction> query,
            ulong guildId, Guid currencyId, ulong userId) => query
            .Include(c => c.Currency)
            .Where(c => c.Currency!.GuildId == guildId)
            .Where(c => c.CurrencyId == currencyId)
            .Where(c => c.SenderId == userId || c.RecipientId == userId);
    }
}
