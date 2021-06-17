using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Id of guild, where the transaction occured.
        /// </summary>
        public ulong GuildId { get; set; }
        
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
        public int AmountTransferred { get; set; }
        
        /// <summary>
        /// Is award. Means that the sender did not lose money in this transaction.
        /// </summary>
        public bool IsAward { get; set; }

        [ForeignKey("GuildId, SenderId")]
        public virtual GuildMember? Sender { get; set; }

        [ForeignKey("GuildId, RecipientId")]
        public virtual GuildMember? Recipient { get; set; }
    }
}