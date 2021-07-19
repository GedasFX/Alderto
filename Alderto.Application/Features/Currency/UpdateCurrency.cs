using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public class Command : CommandRequest<Data.Models.Currency>
        {
            public Guid? Id { get; set; }

            [MaxLength(50)]
            public string? Name { get; }

            [MaxLength(2000)]
            public string? Description { get; init; }

            [MaxLength(50), MinLength(1)]
            public string? Symbol { get; init; }

            public int? TimelyInterval { get; init; }
            public int? TimelyAmount { get; init; }
            public bool? IsLocked { get; set; }

            public Command(ulong guildId, ulong memberId, Guid id)
                : base(guildId, memberId)
            {
                Id = id;
            }

            public Command(ulong guildId, ulong memberId, string name)
                : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Data.Models.Currency>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Data.Models.Currency> Handle(Command request, CancellationToken cancellationToken)
            {
                var query = _context.Currencies.AsQueryable().Where(c => c.GuildId == request.GuildId);
                query = request.Id != null
                    ? query.Where(c => c.Id == request.Id)
                    : query.Where(c => c.Name == request.Name);

                var currency = await query.SingleOrDefaultAsync(cancellationToken);

                if (currency == null)
                    throw new ValidationDomainException(ErrorMessage.CURRENCY_NOT_FOUND);

                _mapper.Map(request, currency);
                _context.Currencies.Update(currency);

                await _context.SaveChangesAsync(cancellationToken);

                return currency;
            }
        }

        private class MapperProfile : Profile
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
