using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildMemberWallet
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Currency Id. FK for <see cref="Currency" />
        /// </summary>
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// Discord Member Id.
        /// </summary>
        public ulong MemberId { get; set; }


        /// <summary>
        /// Amount of currency this user owns.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Time of last currency claim.
        /// </summary>
        public DateTimeOffset TimelyLastClaimed { get; set; }

        [ForeignKey(nameof(MemberId))]
        public virtual Member? Member { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency? Currency { get; set; }

        public GuildMemberWallet(Guid currencyId, ulong memberId, int amount = 0)
        {
            CurrencyId = currencyId;
            MemberId = memberId;
            Amount = amount;
        }
    }
}