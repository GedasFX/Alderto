using System.Collections.Generic;
using System.Linq;
using Alderto.Web.Models.Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("api/users")]
    public class UserController : ApiControllerBase
    {
        private readonly DiscordSocketClient _bot;

        public UserController(DiscordSocketClient bot)
        {
            _bot = bot;
        }

        [HttpPost("@me/mutual-guilds")]
        public IActionResult FilterMutualGuilds(IEnumerable<DiscordGuild> guilds)
        {
            // Json parser sometimes has trouble passing Lists.
            var userGuilds = guilds as ICollection<DiscordGuild> ?? guilds.ToArray();
            if (userGuilds.Count > 100)
                return BadRequest(ErrorMessages.GuildCountOver100);

            // _bot.GetGuild(ulong id) returns null if bot is currently not connected to that guild.
            var mutualGuilds = userGuilds.Where(userGuild => _bot.GetGuild(ulong.Parse(userGuild.Id)) != null);
            return Content(mutualGuilds);
        }
    }
}