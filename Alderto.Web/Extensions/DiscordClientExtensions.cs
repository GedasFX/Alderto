using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace Alderto.Web.Extensions
{
    public static class DiscordClientExtensions
    {
        /// <summary>
        /// Contacts the Discord API to verify that the user is administrator of a given guild.
        /// </summary>
        /// <param name="client">Discord client to check admin status of.</param>
        /// <param name="userId">The Id of user, to check admin status of.</param>
        /// <param name="guildId">The Id of guild to check if user is admin of.</param>
        /// <returns>True if user was confirmed to be admin of a given guild.</returns>
        public static async Task<bool> ValidateGuildAdminAsync(this IDiscordClient client, ulong userId, ulong guildId)
        {
            var guild = await client.GetGuildAsync(guildId);
            if (guild == null)
                return false;

            var user = await guild.GetUserAsync(userId);
            if (user == null)
                return false;

            return user.GuildPermissions.Administrator;
        }

        /// <summary>
        /// Contacts the Discord API to verify that the user is administrator of a given guild.
        /// </summary>
        /// <param name="client">Discord client to check admin status of.</param>
        /// <param name="userId">The Id of user, to check admin status of.</param>
        /// <param name="guildId">The Id of guild to check if user is admin of.</param>
        /// <param name="modRoleId">Id of role which moderates a resource.</param>
        /// <returns>True if user was confirmed to be admin of a given guild.</returns>
        public static async Task<bool> ValidateResourceModeratorAsync(this IDiscordClient client, ulong userId, ulong guildId, ulong? modRoleId)
        {
            var guild = await client.GetGuildAsync(guildId);
            if (guild == null)
                return false;

            var user = await guild.GetUserAsync(userId);
            if (user == null)
                return false;

            // Check if user has moderator role, or is administrator. 
            // Do not iterate thru roles if modrole is undefined.
            return modRoleId != null && user.RoleIds.Any(r => r == modRoleId) || user.GuildPermissions.Administrator;
        }

        /// <summary>
        /// Contacts the Discord API to verify that the user is administrator of a given guild.
        /// </summary>
        /// <param name="client">Discord client to check admin status of.</param>
        /// <param name="userId">The Id of user, to check admin status of.</param>
        /// <param name="guildId">The Id of guild to check if user is admin of.</param>
        /// <param name="modRoleId">Id of role which moderates a resource.</param>
        /// <returns>True if user was confirmed to be admin of a given guild.</returns>
        public static async Task<bool> ValidateInRoleAsync(this IDiscordClient client, ulong userId, ulong guildId, ulong modRoleId)
        {
            var guild = await client.GetGuildAsync(guildId);
            if (guild == null)
                return false;

            var user = await guild.GetUserAsync(userId);
            if (user == null)
                return false;

            return user.RoleIds.Any(r => r == modRoleId);
        }
    }
}