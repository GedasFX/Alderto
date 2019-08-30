using Alderto.Services.GuildBankManagers;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("api/guild/{guildId}/bank/{bankId}")]
    public class BankContentsController : ApiControllerBase
    {
        private readonly IGuildBankItemManager _bankContents;

        public BankContentsController(IGuildBankItemManager bankContents)
        {
            _bankContents = bankContents;
        }

        [HttpGet("contents")]
        public IActionResult Contents(ulong guildId, int bankId)
        {
            return Content(_bankContents.GetGuildBankItems(bankId));
        }
        
    }
}