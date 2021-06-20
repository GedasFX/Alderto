using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency.Query
{
    public class CurrencyTransactions
    {
        public class List : Request<Dto>
        {
            public string CurrencyName { get; }

            public int Page { get; }

            public List(ulong guildId, ulong memberId, string currencyName, int page = 1) : base(guildId, memberId)
            {
                CurrencyName = currencyName;
                Page = page;
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
                return await _mapper.ProjectTo<Dto>(_context.Currencies
                        .Include(c =>
                            c.Transactions.Where(t =>
                                t.SenderId == request.MemberId || t.RecipientId == request.MemberId))
                        .Where(c => c.GuildId == request.GuildId && c.Name == request.CurrencyName))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, Dto>();
                CreateMap<Data.Models.CurrencyTransaction, Dto.TransactionEntry>();
            }
        }
    }
}