using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Alderto.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Alderto.Web.Controllers
{
    [ApiController, Authorize, Route("api/bank")]
    public class BankController : Controller
    {
        private readonly IGuildBankManager _bank;

        public BankController(IGuildBankManager bank)
        {
            _bank = bank;
        }

        [HttpGet, Route("list")]
        public IActionResult ListBanks(ulong guildId)
        {
            _bank.Configure(guildId, 0);
            var banks = _bank.GetAllGuildBanks(o => o.Include(b => b.Contents));

            return Content(JsonConvert.SerializeObject(banks));
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateBank(ulong guildId, string name)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ForbidReason.NotDiscordAdmin);

            _bank.Configure(guildId, User.GetId());

            var b = await _bank.CreateGuildBankAsync(name);
            return Content(JsonConvert.SerializeObject(b));
        }

        [HttpPatch, Route("rename")]
        public async Task<IActionResult> RenameBank(ulong guildId, int bankId, string newName)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ForbidReason.NotDiscordAdmin);

            _bank.Configure(guildId, User.GetId());

            await _bank.UpdateGuildBankAsync(bankId, b => { b.Name = newName; });

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBank(ulong guildId, int bankId)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ForbidReason.NotDiscordAdmin);

            await _bank.RemoveGuildBankAsync(bankId);
            return Ok();
        }


        private IActionResult Forbid(object data)
        {
            return StatusCode(StatusCodes.Status403Forbidden, data);
        }

        private static class ForbidReason
        {
            public const string NotDiscordAdmin = "Could not confirm if user is an admin of the specified server.";
        }
    }
}