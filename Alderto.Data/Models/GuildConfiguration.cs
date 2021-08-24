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
        [MaxLength(20), MinLength(1)]
        public string Prefix { get; set; }

        /// <summary>
        /// Log channel id for logging messages to.
        /// </summary>
        public ulong? LogChannelId { get; set; }

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
