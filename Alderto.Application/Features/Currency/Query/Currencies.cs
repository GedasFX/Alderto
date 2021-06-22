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
        public class List<T> : Request<IList<T>>
        {
            public List(ulong guildId, ulong memberId) : base(guildId, memberId)
            {
            }
        }

        public class FindByName<T> : Request<T?>
        {
            [MaxLength(50)]
            public string Name { get; }

            public FindByName(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }


        public class QueryHandler<T> : IRequestHandler<FindByName<T>, T?>, IRequestHandler<List<T>, IList<T>>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<T?> Handle(FindByName<T> request, CancellationToken cancellationToken)
            {
                return await _mapper
                    .ProjectTo<T>(_context.Currencies.Where(c =>
                        c.GuildId == request.GuildId && c.Name == request.Name))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }

            public async Task<IList<T>> Handle(List<T> request, CancellationToken cancellationToken)
            {
                return await _mapper
                    .ProjectTo<T>(_context.Currencies.Where(c => c.GuildId == request.GuildId))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}