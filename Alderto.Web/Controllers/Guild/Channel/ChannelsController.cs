using System.Linq;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild.Channel
{
    [Route("guilds/{guildId}/channels")]
    public class ChannelsController : ApiControllerBase
    {
        private readonly DiscordSocketClient _client;

        public ChannelsController(DiscordSocketClient client)
        {
            _client = client;
        }

        [HttpGet]
        public IActionResult Channels(ulong guildId)
        {
            if (!User.IsDiscordAdminAsync(_client, guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            return Content(_client.GetGuild(guildId).TextChannels.Select(c => new ApiGuildChannel(c.Id, c.Name)));
        }
    }
}