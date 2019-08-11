using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models.GuildBank
{
    public class GuildBankTransaction
    {
        /// <summary>
        /// Key. Id of the transaction.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Date and time when transaction occured.
        /// </summary>
        public DateTimeOffset TransactionDate { get; set; }

        /// <summary>
        /// Optional comment describing the transaction.
        /// </summary>
        [MaxLength(140)]
        public string Comment { get; set; }

        /// <summary>
        /// Foreign key to the <see cref="GuildBankItem"/> which was modified.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Foreign key to the <see cref="GuildBank"/>, in which the transaction happened.
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// Foreign key to the <see cref="Models.Member"/>, who has initiated the transaction.
        /// </summary>
        public ulong MemberId { get; set; }

        /// <summary>
        /// <see cref="GuildBankItem"/>, which was modified.
        /// </summary>
        [ForeignKey(nameof(ItemId))]
        public virtual GuildBankItem Item { get; set; }

        /// <summary>
        /// <see cref="GuildBank"/>, in which the transaction occured.
        /// </summary>
        [ForeignKey(nameof(BankId))]
        public virtual GuildBank Bank { get; set; }

        /// <summary>
        /// <see cref="Models.Member"/>, who has initiated the transaction.
        /// </summary>
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }
    }
}