using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildManagedMessage
    {
        public ulong GuildId { get; set; }
        public ulong MessageId { get; set; }

        /*
         * NOTE: ChannelId is not part of the primary key, and collisions may occur, as Message Id's are unique within a channel context, not guild context.
         * That being said, the only way to make the message/guild Id combo duplicate, is if one server handles both channel messages of the same guild at the exact same nanosecond,
         * which is rarer than winning a lottery 10 times in a row.
         */
        public ulong ChannelId { get; set; }

        [ForeignKey(nameof(GuildId))]
        public virtual Guild Guild { get; set; }

        public GuildManagedMessage() { }
        public GuildManagedMessage(ulong guildId, ulong channelId, ulong messageId)
        {
            GuildId = guildId;
            ChannelId = channelId;
            MessageId = messageId;
        }
    }
}
