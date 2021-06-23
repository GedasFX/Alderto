using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency.Query
{
    public static class Wallets
    {
        public class FindByName<T> : Request<T>
        {
            [MaxLength(50)]
            public string Name { get; }

            public FindByName(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class CommandHandler<T> : IRequestHandler<FindByName<T>, T>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public CommandHandler(AldertoDbContext context, IMapper mapper, IMediator mediator)
            {
                _context = context;
                _mapper = mapper;
                _mediator = mediator;
            }

            public async Task<T> Handle(FindByName<T> request, CancellationToken cancellationToken)
            {
                var result = await _mapper
                    .ProjectTo<T>(_context.GuildMemberWallets
                        .Include(w => w.Currency)
                        .Where(w =>
                            w.Currency!.Name == request.Name && w.Currency!.GuildId == request.GuildId &&
                            w.MemberId == request.MemberId))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);

                if (result != null)
                    return result;


                // Try to create a wallet for the given user.
                await _mediator.Send(new CreateWallet.Command(request.GuildId, request.MemberId, request.Name),
                    cancellationToken);
                result = await _mapper
                    .ProjectTo<T>(_context.GuildMemberWallets
                        .Include(w => w.Currency)
                        .Where(w =>
                            w.Currency!.Name == request.Name && w.Currency!.GuildId == request.GuildId &&
                            w.MemberId == request.MemberId))
                    .SingleAsync(cancellationToken: cancellationToken);


                return result;
            }
        }
    }
}