using System;
using System.IO;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Alderto.Bot
{
    public class Startup
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;

        public Startup()
        {
            _config = BuildConfig();

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                // Set the log level
                LogLevel = LogSeverity.Debug
            });
        }

        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Add database
                .AddDbContext<IAldertoDbContext, SqliteDbContext>()

                // Add discord socket client
                .AddSingleton(_client)

                // Add command handling services
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()

                // Add logger service
                .AddLogging(lb => { lb.AddConsole(); })
                .AddSingleton<LoggingService>()

                // Add configuration
                .AddSingleton(_config)

                // Build
                .BuildServiceProvider();
        }

        public async Task RunAsync()
        {
            var services = ConfigureServices();

            // Enable logging
            await services.GetService<LoggingService>().InstallLogger();

            // Start bot
            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();
            
            // Install Command handler
            await services.GetRequiredService<CommandHandlingService>().InstallCommandsAsync();

            // Lock main thread to run indefinetly
            await Task.Delay(-1);
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile("config.json.debug")
#else
                .AddJsonFile("config.json")
#endif
                .Build();
        }
    }
}
