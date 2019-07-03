using System;
using System.IO;
using System.Threading.Tasks;
using Alderto.Bot.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Bot
{
    public class Startup
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private IServiceProvider _services;

        public Startup(DiscordSocketClient client = null, CommandService commands = null)
        {
            _commands = commands ?? new CommandService();
            _client = client ?? new DiscordSocketClient();

            _services = ConfigureServices(new ServiceCollection());
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SqliteDbContext>();

            services.AddSingleton(_commands);
            services.AddSingleton(_client);
            
            return services.BuildServiceProvider();
        }

        private static string GetToken()
        {
#if DEBUG
            const string tokenPath = "token.debug";
#else
            const string tokenPath = "token";
#endif
            return File.ReadAllText(tokenPath);
        }

        public async Task Run()
        {
            // Add logger
            _client.Log += Log;

            // Start bot
            await _client.LoginAsync(TokenType.Bot, GetToken());
            await _client.StartAsync();

            // Command handler
            await new CommandHandler(_client, _commands, _services).InstallCommandsAsync();

            // Lock main thread to run indefinetly
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
