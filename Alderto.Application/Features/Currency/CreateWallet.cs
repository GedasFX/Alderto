using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.GuildMember;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using Alderto.Domain.Services;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency
{
    public static class CreateWallet
    {
        public class Command : Request<Guid>
        {
            [MaxLength(50)]
            public string CurrencyName { get; }

            public Command(ulong guildId, ulong memberId, string currencyName) : base(guildId, memberId)
            {
                CurrencyName = currencyName;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Guid>
        {
            private readonly AldertoDbContext _context;
            private readonly IMediator _mediator;

            public CommandHandler(AldertoDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var currency = await _context.Currencies.SingleOrDefaultAsync(c =>
                        c.GuildId == request.GuildId && c.Name == request.CurrencyName,
                    cancellationToken: cancellationToken);

                if (currency == null)
                    throw new BadRequestDomainException($"Currency '{request.CurrencyName}' does not exit");

                // Make sure member is registered.
                await _mediator.Send(new RegisterGuildMember.Command(request.GuildId, request.MemberId),
                    cancellationToken);

                var wallet = await _context.GuildMemberWallets.SingleOrDefaultAsync(w =>
                        w.CurrencyId == currency.Id && w.MemberId == request.MemberId,
                    cancellationToken: cancellationToken);

                if (wallet != null)
                    throw new BadRequestDomainException("Specified wallet already exists");

                wallet = new GuildMemberWallet(currency.Id, request.MemberId);

                _context.GuildMemberWallets.Add(wallet);
                await _context.SaveChangesAsync(cancellationToken);

                return wallet.Id;
            }
        }
    }
}