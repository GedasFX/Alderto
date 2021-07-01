using System;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency
{
    public class GrantTimelyReward
    {
        public class Command : Request<Model>
        {
            public string CurrencyName { get; set; }

            public Command(ulong guildId, ulong memberId, string currencyName) : base(guildId, memberId)
            {
                CurrencyName = currencyName;
            }
        }

        public class Model
        {
            public bool Success { get; }
            public TimeSpan NextClaim { get; }

            public int ReceivedAmount { get; }
            public string CurrencySymbol { get; }

            public Model(bool success, TimeSpan nextClaim, string currencySymbol, int receivedAmount = 0)
            {
                Success = success;
                NextClaim = nextClaim;
                CurrencySymbol = currencySymbol;
                ReceivedAmount = receivedAmount;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Model>
        {
            private readonly AldertoDbContext _context;
            private readonly IMediator _mediator;

            public CommandHandler(AldertoDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                var wallet = await _context.GuildMemberWallets.Include(w => w.Currency)
                    .SingleOrDefaultAsync(w =>
                        w.Currency!.GuildId == request.GuildId && w.Currency.Name == request.CurrencyName &&
                        w.MemberId == request.MemberId, cancellationToken: cancellationToken);

                if (wallet == null)
                {
                    await _mediator.Send(new CreateWallet.Command(request.GuildId, request.MemberId,
                        request.CurrencyName), cancellationToken);
                    wallet = await _context.GuildMemberWallets.Include(w => w.Currency)
                        .SingleOrDefaultAsync(w =>
                            w.Currency!.GuildId == request.GuildId && w.Currency.Name == request.CurrencyName &&
                            w.MemberId == request.MemberId, cancellationToken: cancellationToken);
                }

                // It is intentional that amount is allowed to be negative.
                if (wallet.Currency!.TimelyInterval == null || wallet.Currency.TimelyAmount == 0)
                    throw new ValidationDomainException("This currency does not grant currency on a timely basis");

                var timeRemaining = wallet.TimelyLastClaimed.AddSeconds((int) wallet.Currency.TimelyInterval) -
                                    DateTimeOffset.UtcNow;

                // If time remaining is positive, that means cooldown hasn't expired yet.
                if (timeRemaining.Ticks > 0)
                    return new Model(false, timeRemaining, wallet.Currency.Symbol, 0);

                // Cooldown expired. Update user.
                wallet.TimelyLastClaimed = DateTimeOffset.Now;
                wallet.Amount += wallet.Currency.TimelyAmount;

                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Send(
                    new LogCurrencyTransaction.Command(request.GuildId, request.MemberId, wallet.CurrencyId,
                        request.MemberId, wallet.Currency.TimelyAmount, true), cancellationToken);

                return new Model(true, TimeSpan.FromSeconds((int) wallet.Currency.TimelyInterval),
                    wallet.Currency.Symbol, wallet.Currency.TimelyAmount);
            }
        }
    }
}