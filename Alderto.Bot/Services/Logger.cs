using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Alderto.Bot.Services
{
    // Todo: implement better logger logic.
    public class Logger : ILogger
    {
        private const ulong LoggingChannel = 601142976374374421;

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly Microsoft.Extensions.Logging.ILogger _discordLogger;
        private readonly Microsoft.Extensions.Logging.ILogger _commandsLogger;

        public Logger(DiscordSocketClient client, CommandService commands, ILoggerFactory logger)
        {
            _client = client;
            _commands = commands;
            _discordLogger = logger.CreateLogger("discord");
            _commandsLogger = logger.CreateLogger("commands");
        }

        public Task InstallLogger()
        {
            _client.Log += LogDiscord;
            _commands.Log += LogCommand;

            return Task.CompletedTask;
        }

        private Task LogDiscord(LogMessage message)
        {
            _discordLogger.Log(
                LogLevelFromSeverity(message.Severity),
                eventId: 0,
                message,
                message.Exception,
                delegate { return message.ToString(prependTimestamp: true); });
            return Task.CompletedTask;
        }

        private async Task LogCommand(LogMessage message)
        {
            // Send a command execution exception to the stacktrace channel
            if (message.Exception is CommandException commandException)
            {
                await ((IMessageChannel) _client.GetChannel(LoggingChannel))
                    .SendMessageAsync($"```{commandException.Message}\n{commandException.InnerException}```");
            }

            _commandsLogger.Log(
                LogLevelFromSeverity(message.Severity),
                eventId: 0,
                message,
                message.Exception,
                delegate { return message.ToString(prependTimestamp: true); });
        }

        private static LogLevel LogLevelFromSeverity(LogSeverity severity)
            => (LogLevel)Math.Abs((int)severity - 5);

    }
}