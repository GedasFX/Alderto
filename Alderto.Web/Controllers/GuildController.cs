using System.Linq;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("api/guilds/{guildId}")]
    public class GuildController : ApiControllerBase
    {
        private readonly DiscordSocketClient _client;

        public GuildController(DiscordSocketClient client)
        {
            _client = client;
        }

        [HttpGet("channels")]
        public IActionResult Channels(ulong guildId)
        {
            if (!User.IsDiscordAdminAsync(_client, guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            return Content(_client.GetGuild(guildId).TextChannels.Select(c => new ApiGuildChannel(c.Id, c.Name)));
        }

        [HttpGet("roles")]
        public IActionResult Roles(ulong guildId)
        {
            if (!User.IsDiscordAdminAsync(_client, guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            return Content(_client.GetGuild(guildId).Roles.Select(c => new ApiGuildRole(c.Id, c.Name)));
        }
    }
}