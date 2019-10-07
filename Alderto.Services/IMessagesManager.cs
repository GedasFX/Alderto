﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Alderto.Data.Models;
using Discord;

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
        Task<IMessage> GetMessageAsync(ulong guildId, ulong messageId);

        /// <summary>
        /// Makes a new message in the specified channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="channelId">Id of channel.</param>
        /// <param name="message">Message to post.</param>
        /// <returns>Newly created message.</returns>
        Task<IUserMessage> PostMessageAsync(ulong guildId, ulong channelId, [MaxLength(2000)] string message);

        /// <summary>
        /// Makes a new message in the specified channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="channelId">Id of channel.</param>
        /// <param name="messageId">Id of posted message.</param>
        /// <returns>Newly created message.</returns>
        Task<IUserMessage> ImportMessageAsync(ulong guildId, ulong channelId, ulong messageId);
        
        /// <summary>
        /// Updates the message in the guild channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="messageId">Id of message.</param>
        /// <param name="newMessageContents">Message to replace old message with.</param>
        Task EditMessageAsync(ulong guildId, ulong messageId, [MaxLength(2000)] string newMessageContents);

        /// <summary>
        /// Deletes the message from the discord channel.
        /// </summary>
        /// <param name="guildId">Id of guild the channel is.</param>
        /// <param name="messageId">Id of message.</param>
        Task RemoveMessageAsync(ulong guildId, ulong messageId);
    }
}