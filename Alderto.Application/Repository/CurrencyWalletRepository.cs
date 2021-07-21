using System;
using System.Linq;
using Alderto.Data;
using Alderto.Data.Models;
using AutoMapper;

namespace Alderto.Application.Repository
{
    public class CurrencyWalletRepository
    {
        private readonly IMapper _mapper;
        private readonly IQueryable<GuildMemberWallet> _dbSet;

        public CurrencyWalletRepository(AldertoDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _dbSet = context.GuildMemberWallets.AsQueryable();
        }

        public IQueryable<GuildMemberWallet> List(ulong guildId, Guid currencyId) => _dbSet
            .Where(c => c.Currency!.GuildId == guildId)
            .Where(c => c.CurrencyId == currencyId);

        public IQueryable<T> List<T>(ulong guildId, Guid currencyId) =>
            _mapper.ProjectTo<T>(List(guildId, currencyId));
    }
}
