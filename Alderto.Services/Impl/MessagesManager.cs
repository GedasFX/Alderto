using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Services.Exceptions.BadRequest;
using Alderto.Services.Exceptions.NotFound;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Services.Impl
{
    public class MessagesManager : IMessagesManager
    {
        private readonly DiscordSocketClient _client;
        private readonly AldertoDbContext _context;

        public MessagesManager(DiscordSocketClient client, AldertoDbContext context)
        {
            _client = client;
            _context = context;
        }

        public Task<List<GuildManagedMessage>> ListMessagesAsync(ulong guildId)
        {
            return _context.GuildManagedMessages.Where(m => m.GuildId == guildId).ToListAsync();
        }

        public async Task<IMessage> GetMessageAsync(ulong guildId, ulong messageId)
        {
            var msg = await GetManagedMessage(guildId, messageId);
            return await GetDiscordChannel(guildId, msg.ChannelId).GetMessageAsync(messageId);
        }

        public async Task<IUserMessage> PostMessageAsync(ulong guildId, ulong channelId, string message)
        {
            var discordMessage = await GetDiscordChannel(guildId, channelId).SendMessageAsync(message);

            _context.GuildManagedMessages.Add(new GuildManagedMessage(guildId, channelId, discordMessage.Id));
            await _context.SaveChangesAsync();

            return discordMessage;
        }

        public async Task<IUserMessage> ImportMessageAsync(ulong guildId, ulong channelId, ulong messageId)
        {
            var discordMessage = await GetDiscordBotMessageAsync(guildId, channelId, messageId);

            _context.GuildManagedMessages.Add(new GuildManagedMessage(guildId, channelId, messageId));
            await _context.SaveChangesAsync();

            return discordMessage;
        }

        public async Task EditMessageAsync(ulong guildId, ulong messageId, string newMessageContents)
        {
            var message = await GetDiscordBotMessageAsync(await GetManagedMessage(guildId, messageId));

            // User can always edit its own posts.
            await message.ModifyAsync(msg => msg.Content = newMessageContents);
        }

        public async Task RemoveMessageAsync(ulong guildId, ulong messageId)
        {
            var message = await GetDiscordBotMessageAsync(await GetManagedMessage(guildId, messageId));

            // User can always delete its own posts.
            await message.DeleteAsync();
        }


        private Task<GuildManagedMessage> GetManagedMessage(ulong guildId, ulong messageId)
        {
            return _context.GuildManagedMessages.SingleOrDefaultAsync(message => message.GuildId == guildId && message.MessageId == messageId) ?? throw new MessageNotFoundException();
        }

        private IMessageChannel GetDiscordChannel(ulong guildId, ulong channelId)
        {
            var guild = _client.GetGuild(guildId);
            if (guild == null)
                throw new GuildNotFoundException();

            var channel = guild.GetChannel(channelId);
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
            var channel = GetDiscordChannel(guildId, channelId);

            var message = await channel.GetMessageAsync(messageId);
            if (message == null)
                throw new MessageNotFoundException();

            if (!(message is IUserMessage userMessage))
                throw new BotNotMessageOwnerException();

            return userMessage;
        }
    }
}