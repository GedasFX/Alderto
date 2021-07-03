using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency.Query
{
    public static class CurrencyTransactions
    {
        public class List : PagedQueryRequest<Dto>
        {
            public string CurrencyName { get; }

            public List(ulong guildId, ulong memberId, string currencyName, int page = 1, int take = 25)
                : base(guildId, memberId, page, take)
            {
                CurrencyName = currencyName;
            }
        }

        public class Dto
        {
            public string Name { get; set; }
            public string Symbol { get; set; }
            public IList<TransactionEntry> Transactions { get; set; }

            public class TransactionEntry
            {
                public DateTimeOffset Date { get; set; }
                public ulong SenderId { get; set; }
                public ulong RecipientId { get; set; }
                public int Amount { get; set; }
                public bool IsAward { get; set; }
            }
        }

        public class QueryHandler : IRequestHandler<List, Dto>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Dto> Handle(List request, CancellationToken cancellationToken)
            {
                return await _mapper
                    .ProjectTo<Dto>(_context.Currencies.AsQueryable()
                            .Where(c => c.GuildId == request.GuildId && c.Name == request.CurrencyName),
                        new { memberId = request.MemberId, page = request.Page })
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                ulong memberId = 0;
                int page = 0;
                CreateMap<Data.Models.Currency, Dto>()
                    .ForMember(d => d.Transactions, o => o.MapFrom(s => s.Transactions.Where(t =>
                            t.SenderId == memberId || t.RecipientId == memberId)
                        .OrderByDescending(t => t.Date)
                        .Skip(page * 20)
                        .Take(20)));
                CreateMap<Data.Models.CurrencyTransaction, Dto.TransactionEntry>();
            }
        }
    }
}
