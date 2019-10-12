using System.Linq;
using System.Threading.Tasks;
using Alderto.Services.Exceptions;
using Alderto.Services.Exceptions.Forbid;
using Alderto.Services.Exceptions.NotFound;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild.Channel
{
    [Route("guilds/{guildId}/channels")]
    public class ChannelsController : ApiControllerBase
    {
        private readonly IDiscordClient _client;

        public ChannelsController(IDiscordClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Channels(ulong guildId)
        {
            if (!await _client.ValidateGuildAdmin(User.GetId(), guildId))
                throw new UserNotGuildAdminException();

            var guild = await _client.GetGuildAsync(guildId);
            if (guild == null)
                throw new GuildNotFoundException();

            var channels = await guild.GetTextChannelsAsync();
            
            return Content(channels.Select(c => new ApiGuildChannel(c.Id, c.Name)));
        }
    }
}