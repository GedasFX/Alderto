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
        public IServiceProvider ConfigureServices(IConfiguration config) => new ServiceCollection()
            // Add database
            .AddDbContext<IAldertoDbContext, AldertoDbContext>(builder =>
                builder.UseSqlServer(config["DbConnectionString"]))

            // Add discord socket client
            .AddDiscordClient(LogSeverity.Debug)

            // Add command handling services
            .AddCommandService(RunMode.Sync, ignoreExtraArgs: true)
            .AddCommandHandler()

            // Add managers for various bot modules.
            .AddBotManagers()

            // Add Lua command provider.
            // TODO: Decide if the bot should enable this.
            .AddLuaCommandHandler()

            // Add logger service
            .AddLogging(lb =>
            {
                lb.AddConsole();
            })
            .AddSingleton<Services.ILogger, Logger>()

            // Add configuration
            .AddSingleton(config)

            // Build
            .BuildServiceProvider();

        public async Task RunAsync()
        {
            var config = BuildConfig();
            var services = ConfigureServices(config);

            // Enable logging
            await services.GetService<Services.ILogger>().InstallLogger();

            var client = services.GetService<DiscordSocketClient>();

            // Start bot
            await client.LoginAsync(TokenType.Bot, config["DiscordApp:BotToken"]);
            await client.StartAsync();

            // Install Command handler
            await services.GetRequiredService<ICommandHandler>().InstallCommandsAsync();

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
