using System;
using System.Linq;
using System.Security.Claims;
using Alderto.Web.Models.Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize, Route("api/user")]
    public class UserController : Controller
    {
        private readonly DiscordSocketClient _bot;

        public UserController(DiscordSocketClient bot)
        {
            _bot = bot;
        }

        [HttpGet, Route("mutual-guilds")]
        public IActionResult MutualGuilds()
        {
            var user = _bot.GetUser(Convert.ToUInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var mutualGuilds = user.MutualGuilds.Select(g => new DiscordGuild
            {
                Id = g.Id,
                Name = g.Name,
                Icon = g.IconId
            });
            
            return Content(JsonConvert.SerializeObject(mutualGuilds));
        }
    }
}