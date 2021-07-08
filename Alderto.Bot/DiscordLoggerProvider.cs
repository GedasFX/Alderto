using System.Collections.Concurrent;
using Alderto.Domain.Services;
using Discord;
using Microsoft.Extensions.Logging;

namespace Alderto.Bot
{
    public class DiscordLoggerProvider : ILoggerProvider
    {
        private readonly IDiscordClient _client;
        private readonly IGuildSetupService _guildSetupService;
        private readonly ConcurrentDictionary<string, DiscordLogger> _loggers = new();

        public DiscordLoggerProvider(IDiscordClient client, IGuildSetupService guildSetupService)
        {
            _client = client;
            _guildSetupService = guildSetupService;
        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, _ => new DiscordLogger(_client, _guildSetupService));
    }
}
