using System;
using System.IO;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
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
                LogLevel = LogSeverity.Debug
            });
        }

        public IServiceProvider ConfigureServices() => new ServiceCollection()
            // Add database
            .AddDbContext<IAldertoDbContext, AldertoDbContext>(builder => 
                builder.UseSqlServer(_config["DbConnectionString"]))

            // Add discord socket client
            .AddSingleton(_client)

            // Add command handling services
            .AddSingleton<CommandService>()
            .AddSingleton<ICommandHandlingService, CommandHandlingService>()

            // Add Guild preferences provider
            .AddSingleton<IGuildPreferencesProviderService, GuildPreferencesProviderService>()

            // Add Lua command handler
            .AddSingleton<ICustomCommandProviderService, CustomCommandProviderService>()

            // Add logger service
            .AddLogging(lb => { lb.AddConsole(); })
            .AddSingleton<ILoggingService, LoggingService>()

            // Add configuration
            .AddSingleton(_config)

            // Build
            .BuildServiceProvider();

        public async Task RunAsync()
        {
            var services = ConfigureServices();

            // Enable logging
            await services.GetService<ILoggingService>().InstallLogger();

            // Start bot
            await _client.LoginAsync(TokenType.Bot, _config["DiscordApp:BotToken"]);
            await _client.StartAsync();

            // Install Command handler
            await services.GetRequiredService<ICommandHandlingService>().InstallCommandsAsync();

            // Lock main thread to run indefinitely
            await Task.Delay(-1);
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets("c53fe5d3-16e9-400d-a588-4859345371e5")
                .AddJsonFile("settings.json")
                .AddJsonFile("configuration.json")
                .AddJsonFile("commands.json")
                .Build();
        }
    }
}
