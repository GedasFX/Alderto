using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Alderto.Bot.Services
{
    public class LogService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ILogger _discordLogger;
        private readonly ILogger _commandsLogger;

        public LogService(DiscordSocketClient client, CommandService commands, ILoggerFactory logger)
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
                0,
                message,
                message.Exception,
                delegate { return message.ToString(prependTimestamp: false); });
            return Task.CompletedTask;
        }

        private Task LogCommand(LogMessage message)
        {
            // Return an error message for async commands
            if (message.Exception is CommandException command)
            {
                // Don't risk blocking the logging task by awaiting a message send; ratelimits!?
                var _ = command.Context.Channel.SendMessageAsync($"Error: {command.Message}");
            }

            _commandsLogger.Log(
                LogLevelFromSeverity(message.Severity),
                0,
                message,
                message.Exception,
                delegate { return message.ToString(prependTimestamp: false); });
            return Task.CompletedTask;
        }

        private static LogLevel LogLevelFromSeverity(LogSeverity severity)
            => (LogLevel)Math.Abs((int)severity - 5);

    }
}