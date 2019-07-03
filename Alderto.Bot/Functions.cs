using System.Threading.Tasks;
using Alderto.Bot.Data;
using Discord.WebSocket;

namespace Alderto.Bot
{
    public static class Functions
    {
        public static Task Ping(SocketMessage msg)
        {
            return msg.Channel.SendMessageAsync("Pong!");
        }

        public static Task Give(SocketMessage msg)
        {
            return Task.CompletedTask;
        }
    }
}
