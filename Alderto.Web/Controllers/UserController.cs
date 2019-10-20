using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Services.Exceptions;
using Alderto.Web.Models.Discord;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("users")]
    public class UserController : ApiControllerBase
    {
        private readonly IDiscordClient _client;

        public UserController(IDiscordClient client)
        {
            _client = client;
        }

        [HttpPost("@me/mutual-guilds")]
        public IActionResult FilterMutualGuilds(IEnumerable<DiscordGuild> guilds)
        {
            // Json parser sometimes has trouble passing Lists.
            var userGuilds = guilds as ICollection<DiscordGuild> ?? guilds.ToArray();
            if (userGuilds.Count > 100)
                return BadRequest(ErrorMessages.PayloadOver100);

            // _bot.GetGuild(ulong id) returns null if bot is currently not connected to that guild.
            var mutualGuilds = userGuilds.Where(userGuild => _client.GetGuildAsync(ulong.Parse(userGuild.Id, CultureInfo.InvariantCulture)).Result != null);

            return Content(mutualGuilds);
        }
    }
}