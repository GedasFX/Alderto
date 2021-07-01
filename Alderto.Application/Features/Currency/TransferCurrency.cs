using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency
{
    public static class TransferCurrency
    {
        public class Command : Request
        {
            [Required]
            public IEnumerable<ulong> RecipientIds { get; }

            [Required, MaxLength(50)]
            public string CurrencyName { get; }

            [Required]
            public int Amount { get; }

            public bool IsAward { get; }

            public Command(ulong guildId, ulong memberId, IEnumerable<ulong> recipientIds, string currencyName,
                int amount,
                bool isAward = false)
                : base(guildId, memberId)
            {
                RecipientIds = recipientIds;
                CurrencyName = currencyName;
                Amount = amount;
                IsAward = isAward;
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly AldertoDbContext _context;
            private readonly IMediator _mediator;

            public CommandHandler(AldertoDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var currency = await _context.Currencies.AsQueryable().SingleOrDefaultAsync(c =>
                        c.GuildId == request.GuildId && c.Name == request.CurrencyName,
                    cancellationToken: cancellationToken);

                if (currency == null)
                    throw new ValidationDomainException("Specified currency was not found");

                if (!request.IsAward && currency.IsLocked)
                    throw new ValidationDomainException("This currency is locked and can only be given out by admins");

                var recipientWallets = await _context.GuildMemberWallets.AsQueryable()
                    .Where(w => w.CurrencyId == currency.Id)
                    .Where(w => request.RecipientIds.Any(i => w.MemberId == i))
                    .ToListAsync(cancellationToken: cancellationToken);

                foreach (var recipientId in request.RecipientIds)
                {
                    var recipientWallet = recipientWallets.SingleOrDefault(w => w.MemberId == recipientId);
                    if (recipientWallet == null)
                    {
                        await _mediator.Send(new CreateWallet.Command(request.GuildId, recipientId,
                            request.CurrencyName), cancellationToken);
                        recipientWallet = new GuildMemberWallet(currency.Id, recipientId);
                    }

                    recipientWallet.Amount += request.Amount;

                    if (!request.IsAward)
                    {
                        var senderWallet = await _context.GuildMemberWallets.AsQueryable().SingleOrDefaultAsync(w =>
                                w.MemberId == request.MemberId &&
                                w.CurrencyId == currency.Id,
                            cancellationToken: cancellationToken);

                        if (senderWallet == null)
                            throw new ValidationDomainException(
                                "Sender does not have a wallet associated with the specified currency");

                        if (senderWallet.Amount < request.Amount)
                            throw new ValidationDomainException(
                                $"Sender does not have enough currency. Required - {request.Amount}, available - {senderWallet.Amount}");

                        senderWallet.Amount -= request.Amount;
                    }

                    await _mediator.Send(
                        new LogCurrencyTransaction.Command(request.GuildId, request.MemberId, currency.Id, recipientId,
                            request.Amount, request.IsAward), cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
