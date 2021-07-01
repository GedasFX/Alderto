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
    public static class Currencies
    {
        public class List : Request<IList<Dto>>
        {
            public List(ulong guildId, ulong memberId) : base(guildId, memberId)
            {
            }
        }

        public class FindByName : Request<Dto?>
        {
            [MaxLength(50)]
            public string Name { get; }

            public FindByName(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class Dto
        {
            public Guid Id { get; set; }
            public string Symbol { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public int? TimelyInterval { get; set; }
            public int TimelyAmount { get; set; }
            public bool IsLocked { get; set; }

            public Dto(Guid id, string symbol, string name, string? description, int? timelyInterval, int timelyAmount)
            {
                Id = id;
                Symbol = symbol;
                Name = name;
                Description = description;
                TimelyInterval = timelyInterval;
                TimelyAmount = timelyAmount;
            }
        }

        public class QueryHandler : IRequestHandler<FindByName, Dto?>, IRequestHandler<List, IList<Dto>>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Dto?> Handle(FindByName request, CancellationToken cancellationToken)
            {
                return await _mapper
                    .ProjectTo<Dto>(_context.Currencies.AsQueryable().Where(c =>
                        c.GuildId == request.GuildId && c.Name == request.Name))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }

            public async Task<IList<Dto>> Handle(List request, CancellationToken cancellationToken)
            {
                return await _mapper.ProjectTo<Dto>(_context.Currencies.AsQueryable().Where(c => c.GuildId == request.GuildId))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, Dto>();
            }
        }
    }
}
