using System;
using System.Threading.Tasks;
using Alderto.Domain.Models;

namespace Alderto.Domain.Services
{
    public interface IGuildConfigurationService
    {
        /// <summary>
        /// Gets the guild's preferences. Can never be null.
        /// </summary>
        /// <param name="guildId">Id of guild, to get preferences of</param>
        /// <returns>Guild's specific (or default) preferences.</returns>
        Task<GuildSetup> GetGuildSetupAsync(ulong guildId);

        /// <summary>
        /// Updates the guild preferences.
        /// </summary>
        /// <param name="guildId">Discord guild id.</param>
        /// <param name="changes">Changes to apply to the config.</param>
        Task UpdateGuildSetupAsync(ulong guildId, Action<GuildSetup> changes);
    }
}