using System.ComponentModel.DataAnnotations;
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
    public static class DeleteBankItem
    {
        public class Command : CommandRequest<GuildBankItem>
        {
            [Range(1, int.MaxValue)]
            public int Id { get; }

            [Range(1, int.MaxValue)]
            public int BankId { get; }

            public Command(ulong guildId, ulong memberId, int bankId, int id) : base(guildId, memberId)
            {
                BankId = bankId;
                Id = id;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildBankItem>
        {
            private readonly AldertoDbContext _context;
            private readonly IMediator _mediator;

            public CommandHandler(AldertoDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            private IQueryable<GuildBankItem> GuildBankItems(ulong guildId, int bankId) =>
                _context.GuildBankItems.Include(i => i.GuildBank)
                    .Where(i => i.GuildBank!.GuildId == guildId && i.GuildBankId == bankId);

            public async Task<GuildBankItem> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = await GuildBankItems(request.GuildId, request.BankId)
                    .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);

                if (item == null)
                    throw new ValidationDomainException(ErrorMessage.BANK_ITEM_NOT_FOUND);

                _context.GuildBankItems.Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new BankItemDeletedEvent(item, request), cancellationToken);

                return item;
            }
        }
    }
}
