using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class Currency
    {
        /// <summary>
        /// Key. Currency Id.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Discord guild identifier. Foreign key for <see cref="Models.Guild"/>.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Text/EmoteString used for displaying currency.
        /// </summary>
        [MaxLength(50), MinLength(1)]
        public string Symbol { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        /// <summary>
        /// Timely interval.
        /// Period (in seconds), how frequently users are allowed to claim currency.
        /// If set to null, it means that no user is allowed to claim this currency.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? TimelyInterval { get; set; }

        /// <summary>
        /// Timely amount.
        /// Amount of currency user is allowed to claim within a given <see cref="TimelyInterval"/>
        /// </summary>
        public int TimelyAmount { get; set; }

        /// <summary>
        /// <see cref="Guild"/> of which owns this currency.
        /// </summary>
        [ForeignKey(nameof(GuildId))]
        public virtual Guild? Guild { get; set; }

        public virtual IList<CurrencyTransaction>? Transactions { get; set; }

        public Currency(ulong guildId, string symbol, string name)
        {
            GuildId = guildId;
            Symbol = symbol;
            Name = name;
        }
    }
}