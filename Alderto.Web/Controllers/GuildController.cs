using System.Linq;
using System.Threading.Tasks;
using Alderto.Web.Extensions;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("api/guild")]
    public class GuildController : ApiControllerBase
    {
        private readonly DiscordSocketClient _client;

        public GuildController(DiscordSocketClient client)
        {
            _client = client;
        }

        [Route("{id}/channels")]
        public async Task<IActionResult> Channels(ulong id)
        {
            if (!await User.IsDiscordAdminAsync(id))
                return Forbid(ForbidReason.NotDiscordAdmin);

            return Content(_client.GetGuild(id).TextChannels.Select(c => new { c.Id, c.Name }));
        }
    }
}