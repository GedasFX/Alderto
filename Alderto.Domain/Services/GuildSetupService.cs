using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using Alderto.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Alderto.Domain.Services
{
    public class GuildSetupService : IGuildSetupService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;

        public GuildSetupService(IServiceProvider serviceProvider, IMemoryCache cache)
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

        public async Task UpdateGuildConfigurationAsync(ulong guildId, GuildConfiguration newConfiguration)
        {
            // First get the preferences.
            var setup = await GetGuildSetupAsync(guildId);

            // Then update the database.
            using var scope = _serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<AldertoDbContext>();

            if (newConfiguration.GuildId > 0)
                context.GuildPreferences.Update(newConfiguration);
            else
                context.GuildPreferences.Add(newConfiguration);

            await context.SaveChangesAsync();
            _cache.Remove($"GUILD_CFG:{guildId}");
        }

        public async Task CreateCommandAlias(ulong guildId, string alias, string command)
        {
            using var scope = _serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<AldertoDbContext>();

            var commandAlias = await context.GuildCommandAliases.FindAsync(guildId, alias);
            if (commandAlias == null)
            {
                commandAlias = new GuildCommandAlias(guildId, alias, command);
                context.GuildCommandAliases.Add(commandAlias);
            }
            else
            {
                commandAlias.Command = command;
                context.GuildCommandAliases.Update(commandAlias);
            }


            await context.SaveChangesAsync();

            _cache.Remove($"GUILD_CFG:{guildId}");
        }

        public async Task<GuildCommandAlias> RemoveCommandAlias(ulong guildId, string alias)
        {
            using var scope = _serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<AldertoDbContext>();

            var command = await context.GuildCommandAliases.FindAsync(guildId, alias);

            if (command == null)
                throw new BadRequestDomainException("Requested alias was not found.");

            context.GuildCommandAliases.Remove(command);
            await context.SaveChangesAsync();

            _cache.Remove($"GUILD_CFG:{guildId}");

            return command;
        }
    }
}