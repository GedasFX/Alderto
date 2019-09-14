using Discord.WebSocket;

namespace Alderto.Services
{
    public class NewsProvider
    {
        private readonly DiscordSocketClient _client;

        public NewsProvider(DiscordSocketClient client)
        {
            _client = client;
        }
    }
}