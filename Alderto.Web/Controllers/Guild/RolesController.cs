using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.Discord.Dto;
using Alderto.Application.Features.Discord.Query;
using Alderto.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
    [Route("guilds/{guildId}/roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IList<DiscordRoleDto>> ListRoles(ulong guildId)
        {
            return await _mediator.Send(new Roles.List<DiscordRoleDto>(guildId, User.GetId()));
        }
    }
}
