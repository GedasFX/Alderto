using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildCommandAlias
    {
        public ulong GuildId { get; set; }

        [MaxLength(50)]
        public string Alias { get; set; }

        [Required, MaxLength(2000)]
        public string Command { get; set; }

        [ForeignKey(nameof(GuildId))]
        public virtual Guild? Guild { get; set; }

        public GuildCommandAlias(ulong guildId, string alias, string command)
        {
            GuildId = guildId;
            Alias = alias;
            Command = command;
        }
    }
}