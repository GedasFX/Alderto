using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Alderto.Bot
{
    public class Startup
    {
        public IConfiguration Config { get; }
        public DiscordSocketClient Client { get; }

        public Startup()
        {
            // Configuration for the application
            Config = new ConfigurationBuilder()
#if DEBUG
                .AddJsonFile("config.debug.json", optional: false, reloadOnChange: true)
#else
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
#endif
                .Build();

            // Discord Bot API
            Client = new DiscordSocketClient();

            // Database
            
        }

        public async Task Run()
        {
            // Add logger
            Client.Log += Log;

            // Start bot
            await Client.LoginAsync(TokenType.Bot, Config["Token"]);
            await Client.StartAsync();

            // Event handlers
            Client.MessageReceived += MessageReceivedHandler.OnMessageReceived;
            
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
