using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alderto.Data.Models
{
    public class GuildManagedMessage
    {
        /// <summary>
        /// Key. Id of the guild the message is in.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// Key. Id of discord message this bot is managing.
        /// </summary>
        public ulong MessageId { get; set; }

        /*
         * NOTE: ChannelId is not part of the primary key, and collisions may occur, as Message Id's are unique within a channel context, not guild context.
         * That being said, the only way to make the message/guild Id combo duplicate, is if one server handles both channel messages of the same guild at the exact same nanosecond,
         * which is rarer than winning a lottery 10 times in a row.
         */
        /// <summary>
        /// Id of discord channel this message is in.
        /// </summary>
        public ulong ChannelId { get; set; }

        /// <summary>
        /// Id of discord role, which has edit access to this resource.
        /// </summary>
        public ulong? ModeratorRoleId { get; set; }

        /// <summary>
        /// Last known contents of the message.
        /// </summary>
        [MaxLength(2000)]
        public string? Contents { get; set; }

        /// <summary>
        /// Time and date of when contents was last synced with discord.
        /// </summary>
        public DateTimeOffset? LastUpdate { get; set; }

        /// <summary>
        /// Guild, which manages this resource.
        /// </summary>
        [ForeignKey(nameof(GuildId))]
        public virtual Guild? Guild { get; set; }

        /// <summary>
        /// Initializes a new empty instance of <see cref="GuildManagedMessage"/>.
        /// For use by Entity Framework.
        /// </summary>
        private GuildManagedMessage() { }

        public GuildManagedMessage(ulong guildId, ulong channelId, ulong messageId)
        {
            GuildId = guildId;
            ChannelId = channelId;
            MessageId = messageId;
        }

        public GuildManagedMessage(ulong guildId, ulong channelId, ulong messageId, string contents, DateTimeOffset? lastUpdate)
            : this(guildId, channelId, messageId)
        {
            Contents = contents;
            LastUpdate = lastUpdate ?? DateTimeOffset.UtcNow;
        }
    }
}
