using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildConfiguration
    {
        private const char DefaultPrefix = '.';

        /// <summary>
        /// Creates a new <see cref="GuildConfiguration"/> object with default settings.
        /// </summary>
        public GuildConfiguration()
        {
            Prefix = DefaultPrefix;
        }

        /// <summary>
        /// Discord guild identifier.
        /// </summary>
        [Key, ForeignKey(nameof(Guild))]
        public ulong Id { get; set; }

        /// <summary>
        /// Guild's chosen prefix
        /// </summary>
        public char? Prefix { get; set; }

        public virtual Guild Guild { get; set; }
    }
}