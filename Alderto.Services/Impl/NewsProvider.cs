﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Alderto.Services.Impl
{
    public class NewsProvider : INewsProvider
    {
        private readonly ISocketMessageChannel _channel;

        public NewsProvider(IDiscordClient client, IOptions<DependencyInjection.NewsProviderOptions> options)
        {
            _channel = (ISocketMessageChannel)client.GetChannelAsync(options.Value.NewsChannelId).Result;
        }

        public async Task<IEnumerable<IMessage>> GetLatestNewsAsync(int count)
        {
            if (_channel == null)
                return Enumerable.Empty<IMessage>();

            return await _channel.GetMessagesAsync(count).FlattenAsync();
        }
    }
}