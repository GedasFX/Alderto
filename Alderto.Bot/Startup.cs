using System;
using System.IO;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Discord;
using Discord.Commands;
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
            .AddDiscordSocketClient(config["DiscordApp:BotToken"],
                socketConfig => { socketConfig.LogLevel = LogSeverity.Debug; })

            // Add command handling services
            .AddCommandService(serviceConfig =>
            {
                serviceConfig.DefaultRunMode = RunMode.Sync;
                serviceConfig.IgnoreExtraArgs = true;
            })
            .AddCommandHandler()

            // Add managers for various bot modules.
            .AddBotManagers()
            
            // Add logger service
            .AddLogging(lb => { lb.AddConsole(); })

            // Add configuration
            .AddSingleton(config)

            // Build
            .BuildServiceProvider();

        public async Task RunAsync()
        {
            var config = BuildConfig();
            var services = ConfigureServices(config);

            // Effectively start the bot.
            // Initializes all of the necessary singleton services from the the IServiceProvider.
            // There has to be a better way to do this, but this does the job well enough.
            services.GetService<CommandHandler>();

            // Lock main thread to run indefinitely.
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
