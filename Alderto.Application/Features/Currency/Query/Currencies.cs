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
        public class FindByName : Request<Model?>
        {
            [MaxLength(50)]
            public string Name { get; }

            public FindByName(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class Model
        {
        }

        public class QueryHandler : IRequestHandler<FindByName, Model?>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Model?> Handle(FindByName request, CancellationToken cancellationToken)
            {
                return await _mapper
                    .ProjectTo<Model>(_context.Currencies.Where(c =>
                        c.GuildId == request.GuildId && c.Name == request.Name))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, Model>();
            }
        }
    }
}