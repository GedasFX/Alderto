using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Alderto.Bot
{
    public class Startup
    {
        private string Token { get; }
        public DiscordSocketClient Client { get; }

        public Startup()
        {
            // Get the bot token
            Token = GetConfiguration();

            // Discord Bot API
            Client = new DiscordSocketClient();
        }

        private static string GetConfiguration()
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
            Client.Log += Log;

            // Start bot
            await Client.LoginAsync(TokenType.Bot, Token);
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
