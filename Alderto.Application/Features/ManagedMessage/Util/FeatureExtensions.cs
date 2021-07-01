using System;
using System.Threading.Tasks;
using Discord;

namespace Alderto.Application.Features.ManagedMessage.Util
{
    internal static class FeatureExtensions
    {
        internal static async Task<IMessageChannel> GetDiscordChannel(this IDiscordClient client,
            ulong guildId, ulong channelId)
        {
            // Limit scope by finding guild first.
            var channel = await client.GetChannelAsync(channelId);

            if (channel is not IGuildChannel gc || gc.GuildId != guildId)
                throw new EntryPointNotFoundException("Channel not found");

            if (channel is not IMessageChannel msgChannel)
                throw new EntryPointNotFoundException("Specified channel is not a text channel");

            return msgChannel;
        }

        internal static async Task<IUserMessage> GetBotMessageAsync(this IMessageChannel channel, ulong botUserId, ulong messageId)
        {
            var message = await channel.GetMessageAsync(messageId);
            if (message == null)
                throw new EntryPointNotFoundException("Message not found");

            if (message is not IUserMessage userMessage || userMessage.Author.Id != botUserId)
                throw new EntryPointNotFoundException("Bot is not the owner of the message");

            return userMessage;
        }
    }
}
