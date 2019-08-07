using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Web.Extensions;
using Alderto.Web.Helpers;
using Alderto.Web.Models.Discord;
using Alderto.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize, Route("api/user")]
    public class UserController : Controller
    {
        private readonly DiscordRestBot _bot;

        public UserController(DiscordRestBot bot)
        {
            _bot = bot;
        }

        [HttpGet, Route("mutual-guilds")]
        public async Task<IActionResult> MutualGuilds()
        {
            var userClient = new DiscordRestUser(User.GetDiscordToken());
            var userGuilds = await userClient.GetGuildsAsync();
            var botGuilds = await _bot.GetGuildsAsync();
            
            var jointGuilds = userGuilds.Join(botGuilds, userGuild => userGuild.Id, botGuild => botGuild.Id, (u, b) => u);

            return Content(JsonConvert.SerializeObject(jointGuilds));
        }

        private class CompareGuilds : EqualityComparer<Guild>
        {
            public override bool Equals(Guild x, Guild y)
            {
                return x?.Id == y?.Id;
            }

            public override int GetHashCode(Guild obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}