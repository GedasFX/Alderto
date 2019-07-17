using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildConfiguration
    {
        /// <summary>
        /// Discord guild identifier.
        /// </summary>
        [Key, ForeignKey(nameof(Guild))]
        public ulong Id { get; set; }

        /// <summary>
        /// <see cref="Guild"/>'s chosen prefix
        /// </summary>
        public char? Prefix { get; set; }

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