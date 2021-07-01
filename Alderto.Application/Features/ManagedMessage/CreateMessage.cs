using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.ManagedMessage.Util;
using Alderto.Data;
using Alderto.Data.Models;
using Discord;
using MediatR;

namespace Alderto.Application.Features.ManagedMessage
{
    public class CreateMessage
    {
        public class Command : Request<GuildManagedMessage>
        {
            [Range(1, int.MaxValue)]
            public ulong ChannelId { get; }

            [MaxLength(2000)]
            public string Content { get; }

            public Command(ulong guildId, ulong memberId, ulong channelId, string content) : base(guildId, memberId)
            {
                ChannelId = channelId;
                Content = content;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildManagedMessage>
        {
            private readonly AldertoDbContext _context;
            private readonly IDiscordClient _client;

            public CommandHandler(AldertoDbContext context, IDiscordClient client)
            {
                _context = context;
                _client = client;
            }

            public async Task<GuildManagedMessage> Handle(Command request, CancellationToken cancellationToken)
            {
                var channel = await _client.GetDiscordChannel(request.GuildId, request.ChannelId);
                var discordMessage = await channel.SendMessageAsync(request.Content);

                // Always will result in a new entry, as bot is making a new post.
                var msg = new GuildManagedMessage(request.GuildId, request.ChannelId, discordMessage.Id, discordMessage.Content);

                _context.GuildManagedMessages.Add(msg);
                await _context.SaveChangesAsync(cancellationToken);

                return msg;
            }
        }
    }
}
