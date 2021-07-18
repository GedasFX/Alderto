using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Discord
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/channels")]
    public class ChannelsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ApiGuildChannel>> ListChannels(ulong guildId)
        {
            var guild = HttpContext.GetDiscordGuild();

            var channels = await guild.GetTextChannelsAsync();

            return channels.Select(c => new ApiGuildChannel(c.Id, c.Name));
        }
    }
}
