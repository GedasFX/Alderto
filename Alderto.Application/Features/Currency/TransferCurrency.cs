using System.ComponentModel.DataAnnotations;
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
            public ulong RecipientId { get; }

            [Required, MaxLength(50)]
            public string CurrencyName { get; }

            [Required]
            public int Amount { get; }

            public bool IsAward { get; }

            public Command(ulong guildId, ulong memberId, ulong recipientId, string currencyName, int amount,
                bool isAward = false)
                : base(guildId, memberId)
            {
                RecipientId = recipientId;
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
                var currency = await _context.Currencies.SingleOrDefaultAsync(c =>
                        c.GuildId == request.GuildId && c.Name == request.CurrencyName,
                    cancellationToken: cancellationToken);

                if (currency == null)
                    throw new BadRequestDomainException("Specified currency was not found");

                var recipientWallet = await _context.GuildMemberWallets.SingleOrDefaultAsync(w =>
                        w.MemberId == request.RecipientId &&
                        w.CurrencyId == currency.Id,
                    cancellationToken: cancellationToken);

                if (recipientWallet == null)
                {
                    recipientWallet = new GuildMemberWallet(currency.Id, request.RecipientId);
                    _context.GuildMemberWallets.Add(recipientWallet);
                }

                recipientWallet.Amount += request.Amount;

                if (!request.IsAward)
                {
                    var senderWallet = await _context.GuildMemberWallets.SingleOrDefaultAsync(w =>
                            w.MemberId == request.MemberId &&
                            w.CurrencyId == currency.Id,
                        cancellationToken: cancellationToken);

                    if (senderWallet == null)
                        throw new BadRequestDomainException(
                            "Sender does not have a wallet associated with the specified currency");

                    if (senderWallet.Amount < request.Amount)
                        throw new BadRequestDomainException(
                            $"Sender does not have enough currency. Required - {request.Amount}, available - {senderWallet.Amount}");

                    senderWallet.Amount -= request.Amount;
                }

                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Send(new LogCurrencyTransaction.Command(request.GuildId, request.MemberId,
                    request.RecipientId, request.Amount, request.IsAward), cancellationToken);

                return Unit.Value;
            }
        }
    }
}