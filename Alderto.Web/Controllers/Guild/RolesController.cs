using System.Linq;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
    [Route("api/guilds/{guildId}/roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly DiscordSocketClient _client;

        public RolesController(DiscordSocketClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Roles(ulong guildId)
        {
            if (!User.IsDiscordAdminAsync(_client, guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            return Content(_client.GetGuild(guildId).Roles.Select(c => new ApiGuildRole(c.Id, c.Name)));
        }
    }
}