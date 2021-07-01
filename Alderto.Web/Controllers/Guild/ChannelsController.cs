using System.Linq;
using System.Threading.Tasks;
using Alderto.Domain.Exceptions;
using Alderto.Web.Models;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
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
        public async Task<IActionResult> ListChannels(ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            if (guild == null)
                throw new NotFoundDomainException();

            var channels = await guild.GetTextChannelsAsync();

            return Content(channels.Select(c => new ApiGuildChannel(c.Id, c.Name)));
        }
    }
}
