using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
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
            public Guid Id { get; set; }

            [MaxLength(50)]
            public string? Name { get; set; }

            [MaxLength(2000)]
            public string? Description { get; set; }

            [MaxLength(50), MinLength(1)]
            public string? Symbol { get; set; }

            public bool? TimelyEnabled { get; set; }
            public int? TimelyInterval { get; set; }
            public int? TimelyAmount { get; set; }
            public bool? IsLocked { get; set; }

            public Command(ulong guildId, ulong memberId, Guid id) : base(guildId, memberId)
            {
                Id = id;
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
                var currency = await _context.Currencies.FindItem(request.GuildId, request.Id)
                    .SingleOrDefaultAsync(cancellationToken);
                if (currency == null)
                    throw new ValidationDomainException(ErrorMessage.CURRENCY_NOT_FOUND);

                _mapper.Map(request, currency);
                await _context.SaveChangesAsync(cancellationToken);

                return currency;
            }
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<int?, int>().ConvertUsing((s, d) => s ?? d);
                CreateMap<bool?, bool>().ConvertUsing((s, d) => s ?? d);

                CreateMap<Command, Data.Models.Currency>()
                    .ForMember(d => d.Id, o => o.Ignore())
                    .ForMember(d => d.TimelyInterval,
                        o => o.MapFrom(s => s.TimelyInterval == null || s.TimelyInterval > 0 ? s.TimelyInterval : 0))
                    .ForAllMembers(o => o.Condition((_, _, m) => m != null));
            }
        }
    }
}
