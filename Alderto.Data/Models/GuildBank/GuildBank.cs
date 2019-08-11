using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models.GuildBank
{
    public class GuildBank
    {
        /// <summary>
        /// Key. Guild Bank Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Discord Guild Id.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Name of the bank.
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// Amount of currency the bank has.
        /// </summary>
        public int CurrencyCount { get; set; }
        
        /// <summary>
        /// Guild, referenced by <see cref="GuildId"/>.
        /// </summary>
        [ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }

        /// <summary>
        /// A collection of transactions the bank has completed.
        /// </summary>
        public virtual IEnumerable<GuildBankTransaction> Transactions { get; set; }

        /// <summary>
        /// A collection of items in the bank.
        /// </summary>
        public virtual IEnumerable<GuildBankContent> GuildBankContents { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="GuildBank"/>.
        /// </summary>
        public GuildBank() { }

        /// <summary>
        /// Initializes a new instance of <see cref="GuildBank"/>, with <see cref="GuildId"/>) property set.
        /// </summary>
        /// <param name="guildId">Id of the guild, which owns the guild.</param>
        public GuildBank(ulong guildId)
        {
            GuildId = guildId;
        }
    }
}