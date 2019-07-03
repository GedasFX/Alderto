using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Alderto.Bot
{
    public static class MessageReceivedHandler
    {
        private const char BotTrigger = '$';

        public static Dictionary<string, Func<SocketMessage, Task>> Actions =
            new Dictionary<string, Func<SocketMessage, Task>>();

        static MessageReceivedHandler()
        {
            Actions["$ping"] = Functions.Ping;
        }

        public static async Task OnMessageReceived(SocketMessage message)
        {
            if (message.Content.StartsWith(BotTrigger))
                if (Actions.TryGetValue(message.Content.Split(' ')[0], out var action))
                    await action(message);
        }
    }
}
