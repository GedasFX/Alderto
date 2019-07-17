﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildConfiguration
    {
        /// <summary>
        /// Default value for <see cref="GuildConfiguration.Prefix"/>
        /// </summary>
        public static string DefaultPrefix { get; } = ".";

        /// <summary>
        /// Default value for <see cref="GuildConfiguration.CurrencySymbol"/>
        /// </summary>
        public static string DefaultCurrencySymbol { get; } = "⚽";

        /// <summary>
        /// Default value for <see cref="GuildConfiguration.TimelyRewardQuantity"/>
        /// </summary>
        public static int DefaultTimelyRewardQuantity { get; } = 1;

        /// <summary>
        /// Default value for <see cref="GuildConfiguration.TimelyCooldown"/>
        /// </summary>
        public static int DefaultTimelyCooldown { get; } = 86400; // 24h


        /// <summary>
        /// Discord guild identifier. Primary and foreign key for <see cref="Models.Guild"/>.
        /// </summary>
        [Key, ForeignKey(nameof(Guild))]
        public ulong GuildId { get; set; }

        /// <summary>
        /// Prefix for commands.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Text/EmoteString used for displaying currency.
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Timely currency claim reward quantity.
        /// </summary>
        public int? TimelyRewardQuantity { get; set; }

        /// <summary>
        /// Timely currency claim reward cooldown. This is time measured in seconds.
        /// </summary>
        public int? TimelyCooldown { get; set; }

        /// <summary>
        /// <see cref="Guild"/> of which owns this configuration.
        /// </summary>
        public virtual Guild Guild { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="GuildConfiguration"/>, with configuration defaults
        /// </summary>
        public GuildConfiguration()
        {
        }
    }
}