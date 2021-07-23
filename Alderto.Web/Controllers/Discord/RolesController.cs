using System.Collections.Generic;
using System.Linq;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Discord
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/roles")]
    public class RolesController : ApiControllerBase
    {
        [HttpGet]
        public IEnumerable<ApiDiscordRole> ListRoles(ulong guildId)
        {
            return HttpContext.GetDiscordGuild().Roles
                .Select(r => new ApiDiscordRole(r.Id, r.Name));
        }
    }
}
