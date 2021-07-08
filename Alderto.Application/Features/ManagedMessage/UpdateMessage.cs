using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.ManagedMessage.Util;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Discord;
using MediatR;

namespace Alderto.Application.Features.ManagedMessage
{
    public static class UpdateMessage
    {
        public class Command : CommandRequest<GuildManagedMessage>
        {
            [Range(1, ulong.MaxValue)]
            public ulong MessageId { get; set; }

            [Range(1, ulong.MaxValue)]
            public ulong ChannelId { get; }

            [MaxLength(2000)]
            public string Content { get; }

            public Command(ulong guildId, ulong memberId, ulong messageId, ulong channelId, string content) : base(guildId,
                memberId)
            {
                MessageId = messageId;
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

            private IQueryable<GuildBank> GuildBanks(ulong guildId) =>
                _context.GuildBanks.AsQueryable().Where(b => b.GuildId == guildId);

            public async Task<GuildManagedMessage> Handle(Command request, CancellationToken cancellationToken)
            {
                var dbMsg = await _context.GuildManagedMessages.FindAsync(new object[] { request.GuildId, request.MessageId },
                    cancellationToken);
                if (dbMsg == null)
                    throw new EntryPointNotFoundException("Message not found");

                var channel = await _client.GetDiscordChannel(request.GuildId, request.ChannelId);
                var message = await channel.GetBotMessageAsync(_client.CurrentUser.Id, request.MessageId);

                // User can always edit its own posts.
                await message.ModifyAsync(msg => msg.Content = request.Content);
                dbMsg.Content = request.Content;

                dbMsg.LastModified = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                return dbMsg;
            }
        }
    }
}
