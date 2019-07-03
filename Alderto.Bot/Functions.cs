﻿using System.Threading.Tasks;
using Discord.WebSocket;

namespace Alderto.Bot
{
    public static class Functions
    {
        public static Task Ping(SocketMessage msg)
        {
            return msg.Channel.SendMessageAsync("Pong!");
        }
    }
}