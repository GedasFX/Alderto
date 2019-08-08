using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Web.Helpers;

namespace Alderto.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Contacts the Discord API to verify that the user is administrator of a given guild.
        /// </summary>
        /// <param name="user">Claims Principal.</param>
        /// <param name="guildId">The Id of guild to check if user is admin of.</param>
        /// <returns>True if user was confirmed to be admin of a given guild.</returns>
        public static async Task<bool> IsDiscordAdminAsync(this ClaimsPrincipal user, ulong guildId)
        {
            return await DiscordApi.VerifyAdminAsync(guildId, $"Bearer {user.GetDiscordToken()}");
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
        /// Gets the discord id claim of the user.
        /// </summary>
        /// <param name="user">Claims Principal.</param>
        /// <returns>Discord user id.</returns>
        public static ulong GetId(this ClaimsPrincipal user)
        {
            return ulong.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}