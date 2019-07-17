using System.Threading.Tasks;
using Alderto.Data.Models;

namespace Alderto.Bot.Services
{
    public interface IGuildPreferencesProviderService
    {
        /// <summary>
        /// Gets the guild's preferences. Can never be null.
        /// </summary>
        /// <param name="guildId">Id of guild, to get preferences of</param>
        /// <returns>Guild's specific (or default) preferences.</returns>
        Task<GuildConfiguration> GetPreferencesAsync(ulong guildId);
    }
}