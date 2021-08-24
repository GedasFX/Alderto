using System.Linq;
using System.Threading.Tasks;
using Alderto.Domain.Services;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
    [Route("guilds/{guildId}")]
    public class GuildUserController : ApiControllerBase
    {
        private readonly IGuildSetupService _guildSetupService;

        public GuildUserController(IGuildSetupService guildSetupService)
        {
            _guildSetupService = guildSetupService;
        }

        [HttpGet("@me")]
        [RequireGuildMember]
        public async Task<ApiGuildUserInfo> GetUserInfo(ulong guildId)
        {
            return new()
            {
                AccessLevel = await CalcAccessLevel(guildId, HttpContext.GetDiscordUser())
            };
        }

        private async Task<AccessLevel> CalcAccessLevel(ulong guildId, IGuildUser user)
        {
            if (user.GuildPermissions.Administrator)
                return AccessLevel.Admin;


            var setup = await _guildSetupService.GetGuildSetupAsync(guildId);
            var modRoleId = setup.Configuration.ModeratorRoleId;

            if (modRoleId != null && user.RoleIds.Contains((ulong) modRoleId))
                return AccessLevel.Moderator;

            return AccessLevel.Member;
        }
    }
}
