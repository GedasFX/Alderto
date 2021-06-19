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
        private readonly ICurrencyManager _currencyManager;

        public CurrencyController(ICurrencyManager currencyManager)
        {
            _currencyManager = currencyManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiLeaderboardEntry>>> LeaderboardsAsync(ulong guildId, int take = 100, int skip = 0)
        {
            return new List<ApiLeaderboardEntry>();
        }
    }
}
