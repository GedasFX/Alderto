using System;
using System.IO;
using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Services;
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
            .AddDbContext<AldertoDbContext>(options =>
            {
                options.UseNpgsql(
                    $"Server={config["Database:Host"]};" +
                    $"Port={config["Database:Port"]};" +
                    $"Database={config["Database:Database"]};" +
                    $"UserId={config["Database:Username"]};" +
                    $"Password={config["Database:Password"]};" +
                    config["Database:Extras"],
                    builder => builder.MigrationsAssembly("Alderto.Data"));
            })

            // Add discord socket client
            .AddDiscordSocketClient(config["DiscordAPI:BotToken"],
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

            using (var scope = services.CreateScope())
            {
                Console.Out.WriteLine("Initializing database...");
                using (var context = scope.ServiceProvider.GetRequiredService<AldertoDbContext>())
                {
                    await context.Database.MigrateAsync();
                }
                Console.Out.WriteLine("Database ready!");
            }

            // Effectively start the bot.
            // Initializes all of the necessary singleton services from the the IServiceProvider.
            // There has to be a better way to do this, but this does the job well enough.
            await services.GetService<CommandHandler>().StartAsync();

            // Lock main thread to run indefinitely.
            await Task.Delay(-1);
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("commands.json")
                .AddUserSecrets("c53fe5d3-16e9-400d-a588-4859345371e5")
                .Build();
        }
    }
}
