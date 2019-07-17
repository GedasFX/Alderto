using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public class GuildPreferencesProviderService : IGuildPreferencesProviderService
    {
        private const char DefaultPrefix = '.';

        private readonly IAldertoDbContext _context;
        private readonly Dictionary<ulong, GuildConfiguration> _preferences;

        private readonly GuildConfiguration _defaultConfiguration;

        public GuildPreferencesProviderService(IAldertoDbContext context)
        {
            _context = context;
            _preferences = new Dictionary<ulong, GuildConfiguration>();
            _defaultConfiguration = new GuildConfiguration
            {
                Prefix = DefaultPrefix
            };
        }

        /// <summary>
        /// Tries to get the guild preferences from the cache. If failed, fetches from database. If failed, uses default preferences. 
        /// </summary>
        /// <param name="guildId">Id of guild, to get preferences of</param>
        /// <returns>Guild's specific (or default) preferences.</returns>
        public async Task<GuildConfiguration> GetPreferencesAsync(ulong guildId)
        {
            // Try getting cached configuration
            if (_preferences.TryGetValue(guildId, out var cfg))
                return cfg ?? _defaultConfiguration;

            // Config does not exist in the cache. Check database.
            cfg = await _context.GuildPreferences.FindAsync(guildId);

            // Add configuration (can be null) to the cache.
            _preferences.Add(guildId, cfg);

            return cfg ?? _defaultConfiguration;
        }
    }
}
