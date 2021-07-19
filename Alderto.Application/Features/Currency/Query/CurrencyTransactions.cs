using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Currency.Dto;
using Alderto.Data;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency.Query
{
    public static class CurrencyTransactions
    {
        public class List<TOut> : PagedQueryRequest<TOut>
        {
            public Guid? Id { get; }
            public string CurrencyName { get; }

            public List(ulong guildId, ulong memberId, string currencyName, int page = 1, int take = 25)
                : base(guildId, memberId, page, take)
            {
                CurrencyName = currencyName;
            }

            public List(ulong guildId, ulong memberId, Guid id, int page = 1, int take = 25)
                : base(guildId, memberId, page, take)
            {
                Id = id;
            }
        }

        public class QueryHandler : IRequestHandler<List<CurrencyTransactionDto>, CurrencyTransactionDto>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CurrencyTransactionDto> Handle(List<CurrencyTransactionDto> request,
                CancellationToken cancellationToken)
            {
                return await _mapper
                    .ProjectTo<CurrencyTransactionDto>(_context.Currencies.AsQueryable()
                            .Where(c => c.GuildId == request.GuildId && c.Name == request.CurrencyName),
                        new { memberId = request.MemberId, page = request.Page, take = request.Take })
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                const ulong memberId = 0;
                const int page = 0;
                const int take = 0;

                CreateMap<Data.Models.Currency, CurrencyTransactionDto>()
                    .ForMember(d => d.Transactions, o => o.MapFrom(s => s.Transactions!
                        .Where(t =>
                            t.SenderId == memberId || t.RecipientId == memberId)
                        .OrderByDescending(t => t.Date)
                        .Skip(page * take)
                        .Take(take)));
                CreateMap<Data.Models.CurrencyTransaction, CurrencyTransactionDto.TransactionEntry>();
            }
        }
    }
}
