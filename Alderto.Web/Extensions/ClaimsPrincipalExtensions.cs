using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Web.Helpers;
using Discord.WebSocket;

namespace Alderto.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Contacts the Discord API to verify that the user is administrator of a given guild.
        /// </summary>
        /// <param name="user">Claims Principal.</param>
        /// <param name="client">Discord client to check admin status of.</param>
        /// <param name="guildId">The Id of guild to check if user is admin of.</param>
        /// <returns>True if user was confirmed to be admin of a given guild.</returns>
        public static bool IsDiscordAdminAsync(this ClaimsPrincipal user, DiscordSocketClient client, ulong guildId)
        {
            return client.GetGuild(guildId).GetUser(user.GetId()).GuildPermissions.Administrator;
        }

        /// <summary>
        /// Gets the discord Token claim of the user.
        /// </summary>
        /// <param name="user">Claims Principal.</param>
        /// <returns>Discord token string.</returns>
        public static string GetDiscordToken(this ClaimsPrincipal user)
        {
            return user.FindFirst("discord").Value;
        }

        /// <summary>
        /// Gets the Discord id claim of the user.
        /// </summary>
        /// <param name="user">Claims Principal.</param>
        /// <returns>Discord user id.</returns>
        public static ulong GetId(this ClaimsPrincipal user)
        {
            return ulong.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}