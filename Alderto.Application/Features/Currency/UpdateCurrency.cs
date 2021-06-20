using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency
{
    public static class UpdateCurrency
    {
        public class Command : Request
        {
            [MaxLength(50)]
            public string Name { get; }

            [MaxLength(2000)]
            public string? Description { get; init; }

            [MaxLength(50), MinLength(1)]
            public string? Symbol { get; init; }

            public int? TimelyInterval { get; init; }
            public int? TimelyAmount { get; init; }

            public Command(ulong guildId, ulong memberId, string name)
                : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var currency =
                    await _context.Currencies.SingleOrDefaultAsync(c =>
                        c.GuildId == request.GuildId && c.Name == request.Name, cancellationToken: cancellationToken);

                if (currency == null)
                    throw new BadRequestDomainException($"Currency with the name '{request.Name}' was not found");

                _mapper.Map(request, currency);
                _context.Currencies.Update(currency);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, Data.Models.Currency>()
                    .ForMember(d => d.TimelyInterval,
                        o => o.MapFrom(s => s.TimelyInterval > 0 ? s.TimelyInterval : null))
                    .ForMember(d => d.TimelyAmount,
                        o => o.MapFrom(s => s.TimelyAmount >= 0 ? s.TimelyAmount : 0))
                    .ForAllMembers(o => o.Condition((_, _, m) => m != null));
            }
        }
    }
}