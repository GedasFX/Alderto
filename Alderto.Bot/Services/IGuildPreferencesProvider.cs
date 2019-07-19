using System;
using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public interface IGuildPreferencesProvider
    {
        /// <summary>
        /// Gets the guild's preferences. Can never be null.
        /// </summary>
        /// <param name="guildId">Id of guild, to get preferences of</param>
        /// <returns>Guild's specific (or default) preferences.</returns>
        Task<GuildConfiguration> GetPreferencesAsync(ulong guildId);

        /// <summary>
        /// Updates the guild preferences.
        /// </summary>
        /// <param name="guildId">Discord guild id.</param>
        /// <param name="changes">Changes to apply to the config.</param>
        Task UpdatePreferencesAsync(ulong guildId, Action<GuildConfiguration> changes);
    }
}