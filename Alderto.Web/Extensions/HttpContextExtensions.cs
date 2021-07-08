using System;
using Discord;
using Microsoft.AspNetCore.Http;

namespace Alderto.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static IGuildUser GetDiscordUser(this HttpContext context)
        {
            if (context.Items.TryGetValue("discord_user", out var gObj) && gObj is IGuildUser user)
                return user;

            throw new InvalidOperationException("This feature is only available with the RequireGuildXxAttribute");
        }

        public static IGuild GetDiscordGuild(this HttpContext context)
        {
            if (context.Items.TryGetValue("discord_guild", out var gObj) && gObj is IGuild guild)
                return guild;

            throw new InvalidOperationException("This feature is only available with the RequireGuildXxAttribute");
        }
    }
}
