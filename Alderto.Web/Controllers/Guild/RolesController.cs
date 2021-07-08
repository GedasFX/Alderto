using System.Collections.Generic;
using System.Linq;
using Alderto.Application.Features.Discord.Dto;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public RolesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<DiscordRoleDto> ListRoles(ulong guildId)
        {
            return HttpContext.GetDiscordGuild().Roles
                .Select(r => _mapper.Map<DiscordRoleDto>(r));
        }
    }
}
