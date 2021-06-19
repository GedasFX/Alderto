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
    public static class LogCurrencyTransaction
    {
        public class Command : Request
        {
            [Required]
            public ulong RecipientId { get; }

            [Required]
            public int Amount { get; }

            public bool IsAward { get; }

            public Command(ulong guildId, ulong memberId, ulong recipientId, int amount, bool isAward = false)
                : base(guildId, memberId)
            {
                RecipientId = recipientId;
                Amount = amount;
                IsAward = isAward;
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
                var transaction = _mapper.Map<CurrencyTransaction>(request);

                _context.CurrencyTransactions.Add(transaction);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, CurrencyTransaction>()
                    .ForMember(d => d.SenderId, o => o.MapFrom(s => s.MemberId))
                    .ForMember(d => d.Date, o => o.MapFrom(s => new DateTimeOffset()));
            }
        }
    }
}