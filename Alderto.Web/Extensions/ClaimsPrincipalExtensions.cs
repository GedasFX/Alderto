using System;
using System.Globalization;
using System.Security.Claims;
using Discord.WebSocket;

namespace Alderto.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
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
            return ulong.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value, CultureInfo.InvariantCulture);
        }
    }
}