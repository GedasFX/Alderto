using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildManagedMessage
    {
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong MessageId { get; set; }

        [ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }
    }
}
