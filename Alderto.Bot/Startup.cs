using System;
using System.IO;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Data.Models;
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

            // Register loaded guilds
            //_client.Connected += () => RegisterGuildsAsync(services.GetRequiredService<IAldertoDbContext>());

            // Lock main thread to run indefinetly
            await Task.Delay(-1);
        }

        private async Task RegisterGuildsAsync(IAldertoDbContext dbContext)
        {
            var dbGuilds = dbContext.Guilds;
            foreach (var guild in _client.Guilds)
            {
                await RegisterGuildAsync(dbGuilds, guild.Id);
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task RegisterGuildAsync(DbSet<Guild> dbGuilds, ulong guildId)
        {
            // If guild does not exist in the guild database
            if (await dbGuilds.FindAsync(guildId) == null)
            {
                await dbGuilds.AddAsync(new Guild(guildId));
            }
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
