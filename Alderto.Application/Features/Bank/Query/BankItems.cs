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
    public static class BankItems
    {
        public class List<TOut> : PagedQuery<IList<TOut>>
        {
            [Range(1, int.MaxValue)]
            public int BankId { get; }

            public List(ulong guildId, ulong memberId, int bankId, int page = 1, int take = 30) : base(guildId,
                memberId, page, take)
            {
                BankId = bankId;
            }
        }

        public class Find<TOut> : Request<TOut?>
        {
            [Range(1, int.MaxValue)]
            public int Id { get; }

            [Range(1, int.MaxValue)]
            public int BankId { get; }

            public Find(ulong guildId, ulong memberId, int bankId, int id) : base(guildId, memberId)
            {
                Id = id;
                BankId = bankId;
            }
        }

        public class QueryHandler : IRequestHandler<Find<BankItemDto>, BankItemDto?>,
            IRequestHandler<List<BankItemDto>, IList<BankItemDto>>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BankItemDto?> Handle(Find<BankItemDto> request,
                CancellationToken cancellationToken)
            {
                return await _mapper.ProjectTo<BankItemDto>(_context.GuildBankItems
                        .Include(b => b.GuildBank)
                        .Where(b => b.GuildBank!.GuildId == request.GuildId)
                        .Where(b => b.GuildBankId == request.BankId)
                        .Where(b => b.Id == request.Id))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }

            public async Task<IList<BankItemDto>> Handle(List<BankItemDto> request,
                CancellationToken cancellationToken)
            {
                return await _mapper.ProjectTo<BankItemDto>(_context.GuildBankItems
                        .Include(b => b.GuildBank)
                        .Where(b => b.GuildBank!.GuildId == request.GuildId)
                        .Where(b => b.GuildBankId == request.BankId)
                        .Skip(request.Page * request.Take)
                        .Take(request.Take))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<GuildBankItem, BankItemDto>();
            }
        }
    }
}
