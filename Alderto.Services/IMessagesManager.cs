using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Services
{
    public interface IMessagesManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        Task<List<GuildManagedMessage>> ListMessagesAsync(ulong guildId);

        /// <summary>
        /// Gets a posted message in the specified guild channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="messageId">Id of message.</param>
        /// <returns>Message in the channel, or null if not found.</returns>
        Task<GuildManagedMessage?> GetMessageAsync(ulong guildId, ulong messageId);

        /// <summary>
        /// Makes a new message in the specified channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="channelId">Id of channel.</param>
        /// <param name="message">Message to post.</param>
        /// <param name="moderatorRoleId">Id of role, which has edit access to the resource.</param>
        /// <returns>Newly created message.</returns>
        Task<GuildManagedMessage> PostMessageAsync(ulong guildId, ulong channelId, [MaxLength(2000)] string message, ulong? moderatorRoleId = null);

        /// <summary>
        /// Makes a new message in the specified channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="channelId">Id of channel.</param>
        /// <param name="messageId">Id of posted message.</param>
        /// <param name="moderatorRoleId">Id of role, which has edit access to the resource.</param>
        /// <returns>Newly created message.</returns>
        Task<GuildManagedMessage> ImportMessageAsync(ulong guildId, ulong channelId, ulong messageId, ulong? moderatorRoleId = null);

        /// <summary>
        /// Updates the message in the guild channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="messageId">Id of message.</param>
        /// <param name="content">Contents of the message.</param>
        /// <param name="moderatorRoleId">Id of role, which has edit access to the resource.</param>
        Task EditMessageAsync(ulong guildId, ulong messageId, [MaxLength(2000)] string? content = null, ulong? moderatorRoleId = null);

        /// <summary>
        /// Deletes the message from the discord channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="messageId">Id of message.</param>
        Task RemoveMessageAsync(ulong guildId, ulong messageId);
    }
}