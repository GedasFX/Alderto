using System;
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
    public static class DeleteMessage
    {
        public class Command : CommandRequest<GuildManagedMessage>
        {
            [Range(1, ulong.MaxValue)]
            public ulong Id { get; }

            public Command(ulong guildId, ulong memberId, ulong id) : base(guildId, memberId)
            {
                Id = id;
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
                var dbMsg = await _context.GuildManagedMessages.FindAsync(new object[] { request.GuildId, request.Id },
                    cancellationToken);
                if (dbMsg == null)
                    throw new EntryPointNotFoundException("Message not found");

                try
                {
                    var channel = await _client.GetDiscordChannel(request.GuildId, dbMsg.ChannelId);
                    var message = await channel.GetBotMessageAsync(_client.CurrentUser.Id, request.Id);

                    await message.DeleteAsync();
                }
                catch (EntryPointNotFoundException e)
                {
                    if (e.Message != "Message not found")
                        throw;

                    /* Message was removed by someone else. Ignore. */
                }


                _context.GuildManagedMessages.Remove(dbMsg);
                await _context.SaveChangesAsync(cancellationToken);

                return dbMsg;
            }
        }
    }
}
