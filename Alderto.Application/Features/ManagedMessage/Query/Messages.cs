using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.ManagedMessage.Dto;
using Alderto.Data;
using Alderto.Data.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.ManagedMessage.Query
{
    public static class Messages
    {
        public class List<TOut> : PagedQuery<IList<TOut>>
        {
            public List(ulong guildId, ulong memberId, int page = 1, int take = 25) : base(guildId, memberId, page,
                take)
            {
            }
        }

        public class Find<TOut> : Request<TOut?>
        {
            [Range(1, ulong.MaxValue)]
            public ulong Id { get; }

            public Find(ulong guildId, ulong memberId, ulong id) : base(guildId, memberId)
            {
                Id = id;
            }
        }

        public class QueryHandler : IRequestHandler<Find<ManagedMessageDto>, ManagedMessageDto?>,
            IRequestHandler<List<ManagedMessageDto>, IList<ManagedMessageDto>>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ManagedMessageDto?> Handle(Find<ManagedMessageDto> request,
                CancellationToken cancellationToken)
            {
                return await _mapper.ProjectTo<ManagedMessageDto>(_context.GuildManagedMessages.AsQueryable()
                        .Where(b => b.GuildId == request.GuildId)
                        .Where(m => m.MessageId == request.Id))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }

            public async Task<IList<ManagedMessageDto>> Handle(List<ManagedMessageDto> request,
                CancellationToken cancellationToken)
            {
                return await _mapper.ProjectTo<ManagedMessageDto>(_context.GuildManagedMessages.AsQueryable()
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
                CreateMap<GuildManagedMessage, ManagedMessageDto>();
            }
        }
    }
}
