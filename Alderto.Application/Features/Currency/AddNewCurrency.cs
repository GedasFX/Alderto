using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using AutoMapper;
using MediatR;

namespace Alderto.Application.Features.Currency
{
    public static class AddNewCurrency
    {
        public class Command : Request<Guid>
        {
            [MaxLength(50)]
            public string Name { get; }

            [MaxLength(2000)]
            public string? Description { get; init; }

            [MaxLength(50), MinLength(1), Required]
            public string CurrencySymbol { get; }

            public Command(ulong guildId, ulong memberId, string name, string currencySymbol) : base(guildId, memberId)
            {
                Name = name;
                CurrencySymbol = currencySymbol;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Guid>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var currency = _mapper.Map<Data.Models.Currency>(request);

                _context.Currencies.Add(currency);
                await _context.SaveChangesAsync(cancellationToken);

                return currency.Id;
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, Data.Models.Currency>();
            }
        }
    }
}