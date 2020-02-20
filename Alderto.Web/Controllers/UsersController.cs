using Alderto.Web.Models.Discord;
using Alderto.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alderto.Web.Helpers;
using Discord;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using System.Globalization;

namespace Alderto.Web.Controllers
{
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DiscordHttpClient _discord;
        private readonly IDiscordClient _bot;
        private readonly IDataProtector _protector;

        public UsersController(DiscordHttpClient discord, IDiscordClient bot, IDataProtectionProvider protector)
        {
            _discord = discord;
            _bot = bot;
            _protector = protector.CreateProtector(DataProtectionPurposes.DiscordToken);
        }

        [HttpGet("users/@me"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<DiscordApiUser>> GetMeAsync()
        {
            return await _discord.GetUserAsync(_protector.Unprotect(User.FindFirstValue("discord")));
        }

        [HttpGet("users/@me/guilds"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<DiscordApiGuild>>> GetMyGuildsAsync()
        {
            var userGuilds = await _discord.GetUserGuildsAsync(_protector.Unprotect(User.FindFirstValue("discord")));
            return userGuilds.Where(userGuild => _bot.GetGuildAsync(ulong.Parse(userGuild.Id, CultureInfo.InvariantCulture)).Result != null).ToList();
        }
    }
}
