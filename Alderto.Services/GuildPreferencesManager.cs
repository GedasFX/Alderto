using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;

namespace Alderto.Services
{
    public class GuildPreferencesManager : IGuildPreferencesManager
    {
        private readonly IAldertoDbContext _context;
        private readonly Dictionary<ulong, GuildConfiguration> _preferences;

        public GuildPreferencesManager(IAldertoDbContext context)
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

            // Apply changes.
            changes(config);

            // Continue with the the update
            await UpdatePreferencesAsync(guildId, config);
        }

        public async Task UpdatePreferencesAsync(ulong guildId, GuildConfiguration configuration)
        {
            // If config.GuildId == 0, then it means that the guild uses default preferences.
            // Default preferences are only applied, when the GetPreferencesAsync() cannot find them in database.
            var guildPreferencesPresentInDatabase = configuration.GuildId > 0;

            // Ensure the correct guild Id is applied.
            configuration.GuildId = guildId;

            // Then update the database.
            if (guildPreferencesPresentInDatabase)
            {
                _context.GuildPreferences.Update(configuration);
            }
            else
            {
                await _context.GuildPreferences.AddAsync(configuration);
            }

            await _context.SaveChangesAsync();
        }
    }
}
