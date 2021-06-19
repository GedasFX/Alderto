using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency
{
    public static class GetWallet
    {
        public class QueryByName : Request<Model>
        {
            [MaxLength(50)]
            public string Name { get; }

            public QueryByName(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class Model
        {
            public Model(string currencySymbol)
            {
                CurrencySymbol = currencySymbol;
            }

            public int Amount { get; set; }
            public string CurrencySymbol { get; set; }
        }

        public class CommandHandler : IRequestHandler<QueryByName, Model>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Model> Handle(QueryByName request, CancellationToken cancellationToken)
            {
                return await _mapper
                    .ProjectTo<Model>(_context.GuildMemberWallets
                        .Include(w => w.Currency)
                        .Where(w =>
                            w.Currency!.Name == request.Name && w.Currency!.GuildId == request.GuildId &&
                            w.MemberId == request.MemberId))
                    .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, Model>()
                    .ForMember(d => d.CurrencySymbol, o => o.MapFrom(s => s.Symbol));
            }
        }
    }
}