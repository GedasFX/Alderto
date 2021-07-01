using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank.Dto;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Bank.Query
{
    public static class Banks
    {
        public class List<TOut> : PagedQuery<IList<TOut>>
        {
            public List(ulong guildId, ulong memberId, int page = 1, int take = 30) : base(guildId, memberId, page, take)
            {
            }
        }

        public class Find<TOut> : Request<TOut?>
        {
            public int? Id { get; }

            [MaxLength(32)]
            public string? Name { get; }

            public Find(ulong guildId, ulong memberId, int id) : base(guildId, memberId)
            {
                Id = id;
            }

            public Find(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class QueryHandler : IRequestHandler<Find<BankDto>, BankDto?>,
            IRequestHandler<List<BankBriefDto>, IList<BankBriefDto>>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BankDto?> Handle(Find<BankDto> request, CancellationToken cancellationToken)
            {
                var guildBanks = _context.GuildBanks
                    .Include(b => b.Contents)
                    .Where(b => b.GuildId == request.GuildId);

                GuildBank? bank;
                if (request.Id != null)
                    bank = await guildBanks
                        .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken: cancellationToken);
                else if (request.Name != null)
                    bank = await guildBanks
                        .SingleOrDefaultAsync(b => b.Name == request.Name, cancellationToken: cancellationToken);
                else
                    bank = null;

                return _mapper.Map<BankDto>(bank);
            }

            public async Task<IList<BankBriefDto>> Handle(List<BankBriefDto> request,
                CancellationToken cancellationToken)
            {
                return await _mapper.ProjectTo<BankBriefDto>(_context.GuildBanks.AsQueryable()
                        .Where(b => b.GuildId == request.GuildId)
                        .Skip(request.Page * request.Take)
                        .Take(request.Take))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<GuildBank, BankDto>();
                CreateMap<GuildBank, BankBriefDto>();
            }
        }
    }
}
