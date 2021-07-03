using System;
using Alderto.Data.Models;
using Alderto.Web.Extensions;
using Alderto.Web.Models.GuildPreferences;
using Discord;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Alderto.Application.Features.GuildConfiguration;
using Alderto.Domain.Services;
using Alderto.Web.Attributes;
using MediatR;

namespace Alderto.Web.Controllers.Guild
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/preferences")]
    public class PreferencesController : ApiControllerBase
    {
        private readonly IGuildSetupService _guildSetupService;
        private readonly IMediator _mediator;
        private readonly IDiscordClient _client;

        public PreferencesController(IGuildSetupService guildSetupService, IMediator mediator,
            IDiscordClient discordClient)
        {
            _guildSetupService = guildSetupService;
            _mediator = mediator;
            _client = discordClient;
        }

        [HttpGet]
        public async Task<ActionResult<GuildConfiguration>> GetGuildPreferencesAsync(ulong guildId)
        {
            var preferences = await _guildSetupService.GetGuildSetupAsync(guildId);
            return preferences.Configuration;
        }

        [HttpPatch]
        [RequireGuildAdmin]
        public async Task<ActionResult> UpdateGuildPreferencesAsync(ulong guildId, GuildPreferencesInputModel model)
        {
            // Ensure user has admin rights 
            if (!await _client.ValidateGuildAdminAsync(User.GetId(), guildId))
                throw new NotImplementedException();

            await _mediator.Send(new UpdateGuildConfiguration.Command(guildId, User.GetId(), model.Prefix));

            return Ok();
        }
    }
}
