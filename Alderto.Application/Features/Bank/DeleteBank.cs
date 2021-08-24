using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank.Events;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Bank
{
    public static class DeleteBank
    {
        public class Command : CommandRequest<GuildBank>
        {
            public int Id { get; }

            public Command(ulong guildId, ulong memberId, int id) : base(guildId, memberId)
            {
                Id = id;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildBank>
        {
            private readonly AldertoDbContext _context;
            private readonly IMediator _mediator;

            public CommandHandler(AldertoDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            private IQueryable<GuildBank> GuildBanks(ulong guildId) =>
                _context.GuildBanks.AsQueryable().Where(b => b.GuildId == guildId);

            public async Task<GuildBank> Handle(Command request, CancellationToken cancellationToken)
            {
                var bank = await GuildBanks(request.GuildId)
                    .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken: cancellationToken);

                if (bank == null)
                    throw new ValidationDomainException(ErrorMessage.BANK_NOT_FOUND);

                _context.GuildBanks.Remove(bank);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new BankDeletedEvent(bank, request), cancellationToken);

                return bank;
            }
        }
    }
}
