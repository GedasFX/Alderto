using System.Threading.Tasks;
using Alderto.Services.Exceptions;
using Alderto.Web.Extensions;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("guild/{guildId}/users")]
    public class GuildUserController : ApiControllerBase
    {
        private readonly IDiscordClient _client;

        public GuildUserController(IDiscordClient client)
        {
            _client = client;
        }

        [HttpGet("@me/roles")]
        public async Task<IActionResult> ListMyRoles(ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            if (guild == null)
                throw new GuildNotFoundException();

            var user = await guild.GetUserAsync(User.GetId());
            if (user == null)
                throw new UserNotFoundException();

            return Content(user.RoleIds);
        }

        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> ListUserRoles(ulong guildId, ulong userId)
        {
            if (!await _client.ValidateGuildAdminAsync(User.GetId(), guildId))
                throw new UserNotGuildAdminException();

            var guild = await _client.GetGuildAsync(guildId);
            if (guild == null)
                throw new GuildNotFoundException();

            var user = await guild.GetUserAsync(userId);
            if (user == null)
                throw new UserNotFoundException();

            return Content(user.RoleIds);
        }
    }
}