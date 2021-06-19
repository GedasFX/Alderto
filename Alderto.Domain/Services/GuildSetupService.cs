using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Domain.Services
{
    public class GuildConfigurationService : IGuildConfigurationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;

        public GuildConfigurationService(IServiceProvider serviceProvider, IMemoryCache cache)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
        }

        /// <summary>
        /// Tries to get the guild preferences from the cache. If failed, fetches from database. If failed, uses default preferences. 
        /// </summary>
        /// <param name="guildId">Id of guild, to get preferences of</param>
        /// <returns>Guild's specific (or default) preferences.</returns>
        public async Task<GuildSetup> GetGuildSetupAsync(ulong guildId)
        {
            // Try getting cached configuration
            if (_cache.TryGetValue<GuildSetup>($"GUILD_CFG:{guildId}", out var setup))
                return setup;

            // Config does not exist in the cache. Check database. If does not exist in db - use defaults.
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AldertoDbContext>();
                var guild = await context.Guilds.Include(g => g.Configuration).Include(g => g.Aliases)
                    .SingleOrDefaultAsync(g => g.Id == guildId);

                // Build a new setup based on information from database.
                setup = new GuildSetup(
                    guild?.Configuration ?? new GuildConfiguration(),
                    guild?.Aliases?.Aggregate(new Dictionary<string, string>(), (dictionary, alias) =>
                    {
                        dictionary.Add(alias.Alias, alias.Command);
                        return dictionary;
                    }));
            }

            // Add configuration to the cache. If adding default configuration, property GuildId equals 0.
            _cache.Set($"GUILD_CFG:{guildId}", setup,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));

            return setup;
        }

        public async Task UpdateGuildSetupAsync(ulong guildId, Action<GuildSetup> changes)
        {
            // First get the preferences.
            var config = await GetGuildSetupAsync(guildId);

            // If config.GuildId == 0, then it means that the guild uses default preferences.
            // Default preferences [id == 0] are applied only when the GetPreferencesAsync() cannot find them in database.
            var guildPreferencesPresentInDatabase = config.GuildId > 0;

            // Apply changes to Object.
            changes(config);

            // Continue with the the update

            // Ensure the correct guild Id is applied.
            config.GuildId = guildId;

            // Then update the database.
            using var scope = _serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<AldertoDbContext>();


            if (guildPreferencesPresentInDatabase)
                context.GuildPreferences.Update(config);
            else
                context.GuildPreferences.Add(config);

            await context.SaveChangesAsync();
        }
    }
}