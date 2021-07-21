using System;
using System.Linq;
using Alderto.Data;
using Alderto.Data.Models;
using AutoMapper;

namespace Alderto.Application.Repository
{
    public class CurrencyRepository
    {
        private readonly IMapper _mapper;
        private readonly IQueryable<Currency> _dbSet;

        public CurrencyRepository(AldertoDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _dbSet = context.Currencies.AsQueryable();
        }

        public IQueryable<Currency> List(ulong guildId) => _dbSet.Where(s => s.GuildId == guildId);
        public IQueryable<T> List<T>(ulong guildId) => _mapper.ProjectTo<T>(List(guildId));

        public IQueryable<Currency> Find(ulong guildId, Guid id) => List(guildId).Where(c => c.Id == id);
        public IQueryable<T> Find<T>(ulong guildId, Guid id) => _mapper.ProjectTo<T>(Find(guildId, id));

        public IQueryable<Currency> Find(ulong guildId, string name) => List(guildId).Where(c => c.Name == name);
        public IQueryable<T> Find<T>(ulong guildId, string name) => _mapper.ProjectTo<T>(Find(guildId, name));
    }
}
