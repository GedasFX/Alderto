using Alderto.Web.Models.Discord;
using Alderto.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Alderto.Web.Controllers
{
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DiscordHttpClient _discord;
        private readonly IDataProtector _protector;

        public UsersController(DiscordHttpClient discord, IDataProtectionProvider protector)
        {
            _discord = discord;
            _protector = protector.CreateProtector("DiscordToken");
        }

        [HttpGet("users/@me")]
        public async Task<ActionResult<DiscordApiUser>> GetMeAsync()
        {
            return await _discord.GetUserAsync(_protector.Unprotect(User.FindFirstValue("discord")));
        }

        [HttpGet("users/@me/guilds")]
        public async Task<ActionResult<List<DiscordApiGuild>>> GetMyGuildsAsync()
        {
            return await _discord.GetUserGuildsAsync(_protector.Unprotect(User.FindFirstValue("discord")));
        }
    }
}
