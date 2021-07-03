using System.Collections.Generic;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("guilds/{guildId}/users")]
    public class GuildUserController : ApiControllerBase
    {
        [HttpGet("@me/roles")]
        [RequireGuildMember]
        public IEnumerable<ulong> ListMyRoles(ulong guildId)
        {
            return HttpContext.GetDiscordUser().RoleIds;
        }
    }
}
