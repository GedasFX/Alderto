using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Domain.Services;
using AutoMapper;
using MediatR;

namespace Alderto.Application.Features.Currency
{
    public static class CreateCurrency
    {
        public class Command : CommandRequest<Data.Models.Currency>
        {
            [Required, MinLength(1), MaxLength(50)]
            public string Name { get; }

            [MaxLength(2000)]
            public string? Description { get; init; }

            [Required, MinLength(1), MaxLength(50)]
            public string Symbol { get; }

            public bool IsLocked { get; set; } = true;
            public bool TimelyEnabled { get; set; } = false;

            public int? TimelyInterval { get; set; }
            public int? TimelyAmount { get; set; }

            public Command(ulong guildId, ulong memberId, string name, string symbol) : base(guildId, memberId)
            {
                Name = name;
                Symbol = symbol;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Data.Models.Currency>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;
            private readonly IGuildMemberService _memberService;

            public CommandHandler(AldertoDbContext context, IMapper mapper,
                IGuildMemberService memberService)
            {
                _context = context;
                _mapper = mapper;
                _memberService = memberService;
            }

            public async Task<Data.Models.Currency> Handle(Command request, CancellationToken cancellationToken)
            {
                var currency = _mapper.Map<Data.Models.Currency>(request);

                await _memberService.GetGuildMemberAsync(request.GuildId, request.MemberId);
                _context.Currencies.Add(currency);
                await _context.SaveChangesAsync(cancellationToken);

                return currency;
            }
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, Data.Models.Currency>();
            }
        }
    }
}
