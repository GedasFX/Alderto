using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildConfiguration
    {
        /// <summary>
        /// Discord guild identifier. Primary and foreign key for <see cref="Models.Guild"/>.
        /// </summary>
        [Key, ForeignKey(nameof(Guild))]
        public ulong GuildId { get; set; }

        /// <summary>
        /// <see cref="Guild"/>'s chosen prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// <see cref="Guild"/> of which this is the configuration of
        /// </summary>
        public virtual Guild Guild { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="GuildConfiguration"/>.
        /// </summary>
        public GuildConfiguration()
        {
        }
    }
}