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
    public static class Currencies
    {
        public class List<TOut> : QueryRequest<IList<TOut>>
        {
            public List(ulong guildId, ulong memberId) : base(guildId, memberId)
            {
            }
        }

        public class Find<TOut> : QueryRequest<TOut?>
        {
            public Guid? Id { get; }

            [MaxLength(50)]
            public string? Name { get; }

            public Find(ulong guildId, ulong memberId, Guid id) : base(guildId, memberId)
            {
                Id = id;
            }

            public Find(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class QueryHandler : IRequestHandler<Find<CurrencyDto>, CurrencyDto?>,
            IRequestHandler<List<CurrencyDto>, IList<CurrencyDto>>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CurrencyDto?> Handle(Find<CurrencyDto> request, CancellationToken cancellationToken)
            {
                var query = _context.Currencies.AsQueryable()
                    .Where(c => c.GuildId == request.GuildId);

                query = request.Id != null
                    ? query.Where(c => c.Id == request.Id)
                    : query.Where(c => c.Name == request.Name);

                return await _mapper
                    .ProjectTo<CurrencyDto>(query)
                    .SingleOrDefaultAsync(cancellationToken);
            }

            public async Task<IList<CurrencyDto>> Handle(List<CurrencyDto> request, CancellationToken cancellationToken)
            {
                var query = _context.Currencies.AsQueryable()
                    .Where(c => c.GuildId == request.GuildId);

                return await _mapper
                    .ProjectTo<CurrencyDto>(query)
                    .ToListAsync(cancellationToken);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, CurrencyDto>();
            }
        }
    }
}
