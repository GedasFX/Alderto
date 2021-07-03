using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Guild;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Discord;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Alderto.Application.Features.Bank
{
    public static class CreateBank
    {
        public class Command : CommandRequest<GuildBank>
        {
            [MaxLength(32), Required]
            public string Name { get; }

            public Command(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildBank>
        {
            private readonly AldertoDbContext _context;
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(AldertoDbContext context, IMediator mediator, ILogger<CommandHandler> logger)
            {
                _context = context;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<GuildBank> Handle(Command request, CancellationToken cancellationToken)
            {
                // Ensure foreign key constraint is not violated.
                await _mediator.Send(new RegisterGuildMember.Command(request.GuildId, request.MemberId),
                    cancellationToken);

                var bank = new GuildBank(request.GuildId, request.Name);

                _context.GuildBanks.Add(bank);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.Log(LogLevel.Information, 420, "{GuildId}__{User} has created the '{BankName}' bank",
                    request.GuildId, MentionUtils.MentionUser(request.MemberId), request.Name);

                return bank;
            }
        }
    }
}
