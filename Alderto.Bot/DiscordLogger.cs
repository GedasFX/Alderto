using System;
using Alderto.Domain.Services;
using Discord;
using Microsoft.Extensions.Logging;

namespace Alderto.Bot
{
    public class DiscordLogger : ILogger
    {
        private readonly IDiscordClient _client;
        private readonly IGuildSetupService _guildSetupService;
        public static readonly EventId GuildLogEventId = new(420, "Guild Log Event");

        public DiscordLogger(IDiscordClient client, IGuildSetupService guildSetupService)
        {
            _client = client;
            _guildSetupService = guildSetupService;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (eventId != GuildLogEventId || !IsEnabled(logLevel))
                return;

            var msg = formatter(state, exception);

            var chEndIdx = msg.IndexOf("__", 0, 22, StringComparison.Ordinal);
            if (chEndIdx < 0)
                return;

            if (!ulong.TryParse(msg, out var guildId))
                return;

            var setup = await _guildSetupService.GetGuildSetupAsync(guildId);
            var logChannelId = setup.Configuration.LogChannelId;

            if (logChannelId == null)
                return;

            var channel = await _client.GetChannelAsync((ulong) logChannelId) as IMessageChannel;

            // Do not await.
            channel?.SendMessageAsync(msg);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Information;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null!;
        }
    }
}
