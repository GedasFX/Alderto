using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public class GuildPreferencesProviderService : IGuildPreferencesProviderService
    {
        private readonly IAldertoDbContext _context;
        private readonly Dictionary<ulong, GuildConfiguration> _preferences;

        public GuildPreferencesProviderService(IAldertoDbContext context)
        {
            _context = context;
            _preferences = new Dictionary<ulong, GuildConfiguration>();
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
                return cfg;

            // Config does not exist in the cache. Check database. If does not exist in db - use defaults.
            cfg = await _context.GuildPreferences.FindAsync(guildId) ?? GuildConfiguration.DefaultConfiguration;

            // Add configuration to the cache. If adding default configuration, property GuildId equals 0.
            _preferences.Add(guildId, cfg);

            return cfg;
        }

        public async Task UpdatePreferencesAsync(ulong guildId, Action<GuildConfiguration> changes)
        {
            // First get the preferences.
            var config = await GetPreferencesAsync(guildId);

            // If config.GuildId == 0, then it means that the guild uses default preferences.
            // Default preferences are only applied, when the GetPreferencesAsync() cannot find them in database.
            var guildPreferencesPresentInDatabase = config.GuildId > 0;

            // Apply changes.
            changes(config);

            // Apply correct guildId. Do this after applying changes, as changes can modify GuildId.
            config.GuildId = guildId;

            // Then update the database.
            if (guildPreferencesPresentInDatabase)
            {
                _context.GuildPreferences.Update(config);
            }
            else
            {
                await _context.GuildPreferences.AddAsync(config);
            }

            await _context.SaveChangesAsync();
        }
    }
}
