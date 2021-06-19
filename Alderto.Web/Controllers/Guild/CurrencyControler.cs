using Alderto.Services;
using Alderto.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alderto.Web.Controllers.Guild
{
    [Route("guilds/{guildId}/points")]
    public class CurrencyController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiLeaderboardEntry>>> LeaderboardsAsync(ulong guildId,
            int take = 100, int skip = 0)
        {
            return new List<ApiLeaderboardEntry>();
        }
    }
}