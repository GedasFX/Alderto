﻿using System.Linq;
using System.Threading.Tasks;
using Alderto.Services.Exceptions;
using Alderto.Web.Models;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
    [Route("guilds/{guildId}/roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly IDiscordClient _client;

        public RolesController(IDiscordClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles(ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            if (guild == null)
                throw new GuildNotFoundException();

            return Content(guild.Roles.Select(c => new ApiGuildRole(c.Id, c.Name)));
        }
    }
}