using System.Threading.Tasks;
using Alderto.Services.Exceptions.BadRequest;
using Alderto.Services.Exceptions.NotFound;
using Discord;
using Discord.WebSocket;

namespace Alderto.Services
{
    public class MessagesManager : IMessagesManager
    {
        private readonly DiscordSocketClient _client;

        public MessagesManager(DiscordSocketClient client)
        {
            _client = client;
        }

        public Task<IUserMessage> PostMessageAsync(ulong guildId, ulong channelId, string message)
        {
            return GetChannel(guildId, channelId).SendMessageAsync(message);
        }

        public Task<IMessage> GetMessageAsync(ulong guildId, ulong channelId, ulong messageId)
        {
            return GetChannel(guildId, channelId).GetMessageAsync(messageId);
        }

        public async Task EditMessageAsync(ulong guildId, ulong channelId, ulong messageId, string newMessageContents)
        {
            var message = await GetBotMessageAsync(guildId, channelId, messageId);

            // User can always edit its own posts.
            await message.ModifyAsync(msg => msg.Content = newMessageContents);
        }

        public async Task RemoveMessageAsync(ulong guildId, ulong channelId, ulong messageId)
        {
            var message = await GetBotMessageAsync(guildId, channelId, messageId);

            // User can always delete its own posts.
            await message.DeleteAsync();
        }

        private IMessageChannel GetChannel(ulong guildId, ulong channelId)
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

        private async Task<IUserMessage> GetBotMessageAsync(ulong guildId, ulong channelId, ulong messageId)
        {
            var channel = GetChannel(guildId, channelId);

            var message = await channel.GetMessageAsync(messageId);
            if (message == null)
                throw new MessageNotFoundException();

            if (!(message is IUserMessage userMessage))
                throw new BotNotMessageOwnerException();

            return userMessage;
        }
    }
}