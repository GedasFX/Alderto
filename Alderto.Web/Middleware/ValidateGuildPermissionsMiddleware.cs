using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Domain.Services;
using Alderto.Web.Attributes;
using Discord;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Alderto.Web.Middleware
{
    public class ValidateGuildPermissionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDiscordClient _discord;
        private readonly IGuildSetupService _guildSetupService;

        public ValidateGuildPermissionsMiddleware(RequestDelegate next,
            IDiscordClient discord, IGuildSetupService guildSetupService)
        {
            _next = next;
            _discord = discord;
            _guildSetupService = guildSetupService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var res = await ValidateRequest(context);
            if (res == null)
                await _next.Invoke(context);
            else
                context.Response.StatusCode = (int) res;
        }

        // Returns status code based on validation. Returns null if user is allowed to access resource.
        private async Task<int?> ValidateRequest(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
                return null;

            var attrAdmin = endpoint.Metadata.GetMetadata<RequireGuildAdminAttribute>();
            var attrModerator = endpoint.Metadata.GetMetadata<RequireGuildModeratorAttribute>();
            var attrMember = endpoint.Metadata.GetMetadata<RequireGuildMemberAttribute>();

            if (attrAdmin == null && attrModerator == null && attrMember == null)
                return null;


            // At least one of the attributes were present. User MUST be part of the guild at the minimum.

            if (context.GetRouteData().Values["guildId"] is not string gid || !ulong.TryParse(gid, out var guildId))
                throw new InvalidOperationException("Guild id was not found in route");

            if (!ulong.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return StatusCodes.Status401Unauthorized;

            if (!context.User.FindFirstValue("gid").Split(',').Contains(gid))
                return StatusCodes.Status403Forbidden;

            // This can only happen if bot was removed from guild and token is still valid.
            var guild = await _discord.GetGuildAsync(guildId);
            if (guild == null)
                return StatusCodes.Status404NotFound;

            // This can only happen if user was removed from guild and token is still valid.
            var user = await guild.GetUserAsync(userId);
            if (user == null)
                return StatusCodes.Status403Forbidden;

            context.Items.Add("discord_guild", guild);
            context.Items.Add("discord_user", user);


            // If user is an admin, exit early - he has full control no matter what.
            if (user.GuildPermissions.Administrator)
                return null;

            // User is not admin and admin attribute is present. Forbid.
            if (attrAdmin != null)
                return StatusCodes.Status403Forbidden;

            if (attrModerator != null)
            {
                var setup = await _guildSetupService.GetGuildSetupAsync(guildId);
                var modRoleId = setup.Configuration.ModeratorRoleId;

                if (modRoleId != null && user.RoleIds.Contains((ulong) modRoleId))
                    return null;

                // User either did not have the role or moderation access level is disabled.
                return StatusCodes.Status403Forbidden;
            }

            // User has to be part of the guild.
            return null;
        }
    }
}
