using System;
using System.Linq;
using Alderto.Data;
using Alderto.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Repository
{
    public class CurrencyTransactionRepository
    {
        private readonly IMapper _mapper;
        private readonly IQueryable<CurrencyTransaction> _dbSet;

        public CurrencyTransactionRepository(AldertoDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _dbSet = context.CurrencyTransactions.AsQueryable();
        }

        public IQueryable<CurrencyTransaction> List(ulong guildId, Guid currencyId, ulong userId) => _dbSet
            .Include(c => c.Currency)
            .Where(c => c.Currency!.GuildId == guildId)
            .Where(c => c.CurrencyId == currencyId)
            .Where(c => c.SenderId == userId || c.RecipientId == userId);

        public IQueryable<T> List<T>(ulong guildId, Guid currencyId, ulong userId) =>
            _mapper.ProjectTo<T>(List(guildId, currencyId, userId));
    }
}
