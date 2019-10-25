using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Services.Exceptions;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.Impl
{
    public class MessagesManager : IMessagesManager
    {
        private readonly IDiscordClient _client;
        private readonly AldertoDbContext _context;

        public MessagesManager(IDiscordClient client, AldertoDbContext context)
        {
            _client = client;
            _context = context;
        }

        public Task<List<GuildManagedMessage>> ListMessagesAsync(ulong guildId)
        {
            return _context.GuildManagedMessages.Where(m => m.GuildId == guildId).ToListAsync();
        }

        public async Task<GuildManagedMessage?> GetMessageAsync(ulong guildId, ulong messageId)
        {
            return await _context.GuildManagedMessages.FindAsync(guildId, messageId);
        }

        public async Task<GuildManagedMessage> PostMessageAsync(ulong guildId, ulong channelId, string message, ulong? moderatorRoleId = null)
        {
            var channel = await GetDiscordChannel(guildId, channelId);
            var discordMessage = await channel.SendMessageAsync(message);

            // Always will result in a new entry, as bot is making a new post.
            var msg = new GuildManagedMessage(guildId, channelId, discordMessage.Id, discordMessage.Content, moderatorRoleId: moderatorRoleId);

            _context.GuildManagedMessages.Add(msg);
            await _context.SaveChangesAsync();

            return msg;
        }

        public async Task<GuildManagedMessage> ImportMessageAsync(ulong guildId, ulong channelId, ulong messageId, ulong? moderatorRoleId = null)
        {
            var dbMsg = await _context.GuildManagedMessages.SingleOrDefaultAsync(m =>
                m.GuildId == guildId && m.MessageId == messageId);
            if (dbMsg != null)
                return dbMsg;

            // Massage was not yet imported.
            var discordMessage = await GetDiscordBotMessageAsync(guildId, channelId, messageId);

            _context.GuildManagedMessages.Add(dbMsg = new GuildManagedMessage(guildId, channelId, messageId,
                discordMessage.Content, discordMessage.EditedTimestamp ?? discordMessage.CreatedAt, moderatorRoleId));
            await _context.SaveChangesAsync();

            return dbMsg;
        }

        public async Task EditMessageAsync(ulong guildId, ulong messageId, string? content = null, ulong? moderatorRoleId = null)
        {
            var dbMsg = await GetMessageAsync(guildId, messageId);
            if (dbMsg == null)
                throw new MessageNotFoundException();

            var message = await GetDiscordBotMessageAsync(dbMsg);

            // User can always edit its own posts.
            if (content != null)
            {
                await message.ModifyAsync(msg => msg.Content = content);
                dbMsg.Content = content;
            }

            if (moderatorRoleId != null)
                // If edited, check if was edited to not be removed.
                dbMsg.ModeratorRoleId = moderatorRoleId == 0 ? null : moderatorRoleId;

            dbMsg.LastModified = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveMessageAsync(ulong guildId, ulong messageId)
        {
            var dbMsg = await GetMessageAsync(guildId, messageId);
            if (dbMsg == null)
                throw new MessageNotFoundException();

            // Try to delete message from discord.
            try
            {
                // User can always delete its own posts.
                var message = await GetDiscordBotMessageAsync(dbMsg);
                await message.DeleteAsync();
            } catch (MessageNotFoundException) { /* Message is already gone from discord. Ignore error. */ }
            

            _context.GuildManagedMessages.Remove(dbMsg);
            await _context.SaveChangesAsync();
        }

        private async Task<IMessageChannel> GetDiscordChannel(ulong guildId, ulong channelId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            if (guild == null)
                throw new GuildNotFoundException();

            var channel = await guild.GetChannelAsync(channelId);
            if (channel == null)
                throw new ChannelNotFoundException();

            if (!(channel is IMessageChannel msgChannel))
                throw new ChannelNotMessageChannelException();

            return msgChannel;
        }

        private Task<IUserMessage> GetDiscordBotMessageAsync(GuildManagedMessage msg)
        {
            return GetDiscordBotMessageAsync(msg.GuildId, msg.ChannelId, msg.MessageId);
        }

        private async Task<IUserMessage> GetDiscordBotMessageAsync(ulong guildId, ulong channelId, ulong messageId)
        {
            var channel = await GetDiscordChannel(guildId, channelId);

            var message = await channel.GetMessageAsync(messageId);
            if (message == null)
                throw new MessageNotFoundException();

            if (!(message is IUserMessage userMessage) || userMessage.Author.Id != _client.CurrentUser.Id)
                throw new BotNotMessageOwnerException();

            return userMessage;
        }
    }
}