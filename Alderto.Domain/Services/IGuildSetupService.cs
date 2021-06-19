using System.Threading.Tasks;
using Alderto.Data.Models;
using Alderto.Domain.Models;

namespace Alderto.Domain.Services
{
    public interface IGuildSetupService
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
        /// <param name="newConfiguration">New configuration.</param>
        Task UpdateGuildConfigurationAsync(ulong guildId, GuildConfiguration newConfiguration);

        Task CreateCommandAlias(ulong guildId, string alias, string command);
        Task<GuildCommandAlias> RemoveCommandAlias(ulong guildId, string alias);
    }
}