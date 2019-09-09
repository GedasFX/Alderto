using System;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Alderto.Bot
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds a singleton instance of <see cref="DiscordSocketClient"/> to the service collection. Can specify a Log Level.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        /// <param name="botToken">Token of the bot.</param>
        /// <param name="config">Additional options to configure bot with.</param>
        public static IServiceCollection AddDiscordSocketClient(this IServiceCollection services,
            string botToken, Action<DiscordSocketConfig> config = null) =>
            services.AddSingleton<DiscordSocketClient, DiscordSocketClientWrapper>()
                .Configure<DiscordSocketConfigWrapper>(localConfig =>
                {
                    localConfig.BotToken = botToken;
                    config?.Invoke(localConfig);
                });

        private class DiscordSocketConfigWrapper : DiscordSocketConfig
        {
            public string BotToken { get; set; }
        }
        private class DiscordSocketClientWrapper : DiscordSocketClient
        {
            public DiscordSocketClientWrapper(ILogger<DiscordSocketClient> logger,
                IOptions<DiscordSocketConfigWrapper> config) : base(config.Value)
            {
                Log += message =>
                {
                    logger.Log(
                        (LogLevel)Math.Abs((int)message.Severity - 5),
                        eventId: 0,
                        message,
                        message.Exception,
                        delegate { return message.ToString(); });
                    return Task.CompletedTask;
                };

                Run(config.Value.BotToken).ConfigureAwait(false);
            }

            private async Task Run(string token)
            {
                await LoginAsync(TokenType.Bot, token);
                await StartAsync();
            }
        }

        /// <summary>
        /// Adds a singleton instance of <see cref="CommandService"/> to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        /// <param name="config">Additional options to configure bot with.</param>
        public static IServiceCollection AddCommandService(this IServiceCollection services,
            Action<CommandServiceConfig> config = null) =>
            services.AddSingleton<CommandService, CommandServiceWrapper>()
                .Configure<CommandServiceConfig>(localConfig =>
                {
                    config?.Invoke(localConfig);
                });

        private class CommandServiceWrapper : CommandService
        {
            public CommandServiceWrapper(DiscordSocketClient client, ILogger<CommandService> logger,
                IOptions<CommandServiceConfig> cmdServiceConfig, IConfiguration config) : base(cmdServiceConfig.Value)
            {
                if (ulong.TryParse(config["LoggingChannelId"], out var loggingChannelId))
                {
                    Log += async message =>
                    {
                        if (message.Exception is CommandException commandException)
                        {
                            await ((IMessageChannel)client.GetChannel(loggingChannelId))
                                .SendMessageAsync(
                                    $"```{commandException.Message}\n{commandException.InnerException}```");
                        }
                    };
                }

                Log += message =>
                {
                    logger.Log(
                        (LogLevel)Math.Abs((int)message.Severity - 5),
                        eventId: 0,
                        message,
                        message.Exception,
                        delegate { return message.ToString(); });

                    return Task.CompletedTask;
                };
            }
        }

        /// <summary>
        /// Adds a singleton instance of <see cref="CommandHandler"/> to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add to.</param>
        public static IServiceCollection AddCommandHandler(this IServiceCollection services) =>
            services.AddSingleton<CommandHandler>();
    }
}