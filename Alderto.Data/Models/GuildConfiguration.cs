using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildConfiguration
    {
        public static class Defaults
        {
            /// <summary>
            /// Default value for <see cref="Prefix"/>
            /// </summary>
            public const string Prefix = ".";

            /// <summary>
            /// Default value for <see cref="CurrencySymbol"/>
            /// </summary>
            public const string CurrencySymbol = "⚽";

            /// <summary>
            /// Default value for <see cref="TimelyRewardQuantity"/>
            /// </summary>
            public const int TimelyRewardQuantity = 1;

            /// <summary>
            /// Default value for <see cref="TimelyCooldown"/>
            /// </summary>
            public const int TimelyCooldown = 86400; // 24h
        }


        /// <summary>
        /// Discord guild identifier. Primary and foreign key for <see cref="Models.Guild"/>.
        /// If GuildId > 0, the configuration is determined to be in the database. Make sure you know what you are doing modifying this value.
        /// </summary>
        [Key, ForeignKey(nameof(Guild))]
        public ulong GuildId { get; set; }

        /// <summary>
        /// Prefix for commands.
        /// </summary>
        [MaxLength(20), MinLength(1), Required]
        public string Prefix { get; set; }

        /// <summary>
        /// Text/EmoteString used for displaying currency.
        /// </summary>
        [MaxLength(50), MinLength(1), Required]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Timely currency claim reward quantity.
        /// </summary>
        public int TimelyRewardQuantity { get; set; }

        /// <summary>
        /// Timely currency claim reward cooldown. This is time measured in seconds.
        /// </summary>
        public int TimelyCooldown { get; set; }

        /// <summary>
        /// Id of role to add the user to, whenever user was accepted to the guild.
        /// </summary>
        public ulong? ModeratorRoleId { get; set; }

        /// <summary>
        /// <see cref="Guild"/> of which owns this configuration.
        /// </summary>
        public virtual Guild? Guild { get; set; }

        public GuildConfiguration(string prefix = Defaults.Prefix, ulong? moderatorRoleId = null)
        {
            Prefix = prefix;
            ModeratorRoleId = moderatorRoleId;
        }
    }
}