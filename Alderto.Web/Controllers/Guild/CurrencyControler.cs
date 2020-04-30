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
            if (take > 100)
                take = 100;

            var theRich = await _currencyManager.GetRichestUsersAsync(guildId, take, skip);
            return theRich.Select(r => new ApiLeaderboardEntry
            {
                MemberId = r.MemberId,
                Name = r.Nickname,
                Points = r.CurrencyCount,
                LastClaimed = r.CurrencyLastClaimed
            }).ToList();
        }
    }
}
