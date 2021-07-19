using System;
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
            [MaxLength(50)]
            public string Name { get; }

            [MaxLength(2000)]
            public string? Description { get; init; }

            [MaxLength(50), MinLength(1), Required]
            public string Symbol { get; }

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
