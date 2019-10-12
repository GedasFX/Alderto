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
        public static async Task<bool> ValidateGuildAdmin(this IDiscordClient client, ulong userId, ulong guildId)
        {
            var guild = await client.GetGuildAsync(guildId);
            if (guild == null)
                return false;

            var user = await guild.GetUserAsync(userId);

            return user != null && user.GuildPermissions.Administrator;
        }
    }
}