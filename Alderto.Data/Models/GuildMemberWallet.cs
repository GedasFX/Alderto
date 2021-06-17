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
        /// Key. Discord Guild Id in which member is member of.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Discord Member Id.
        /// </summary>
        public ulong MemberId { get; set; }

        /// <summary>
        /// Currency Id. FK for <see cref="Currency" />
        /// </summary>
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// Amount of currency this user owns.
        /// </summary>
        public int Count { get; set; }

        [ForeignKey("GuildId, MemberId")]
        public virtual GuildMember? GuildMember { get; set; }
        
        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency? Currency { get; set; }
    }
}