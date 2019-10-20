using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Services.Impl
{
    public class GuildPreferencesProvider : IGuildPreferencesProvider
    {
        private readonly IServiceProvider _services;
        private readonly Dictionary<ulong, GuildConfiguration> _preferences;

        public GuildPreferencesProvider(IServiceProvider services)
        {
            _services = services;
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
            using (var scope = _services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AldertoDbContext>();
                cfg = await context.GuildPreferences.FindAsync(guildId) ?? GuildConfiguration.DefaultConfiguration;
            }

            // Add configuration to the cache. If adding default configuration, property GuildId equals 0.
            _preferences.Add(guildId, cfg);

            return cfg;
        }

        public async Task UpdatePreferencesAsync(ulong guildId, Action<GuildConfiguration> changes)
        {
            // First get the preferences.
            var config = await GetPreferencesAsync(guildId);

            // If config.GuildId == 0, then it means that the guild uses default preferences.
            // Default preferences [id == 0] are applied only when the GetPreferencesAsync() cannot find them in database.
            var guildPreferencesPresentInDatabase = config.GuildId > 0;

            // Apply changes to Object.
            changes(config);

            // Continue with the the update
            
            // Ensure the correct guild Id is applied.
            config.GuildId = guildId;

            // Then update the database.
            using var scope = _services.CreateScope();
            using var context = scope.ServiceProvider.GetService<AldertoDbContext>();
            if (guildPreferencesPresentInDatabase)
            {
                context.GuildPreferences.Update(config);
            }
            else
            {
                context.GuildPreferences.Add(config);
            }

            await context.SaveChangesAsync();
        }
    }
}
