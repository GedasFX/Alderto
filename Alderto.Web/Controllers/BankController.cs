using System.Threading.Tasks;
using Alderto.Data.Exceptions;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Alderto.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers
{
    [Authorize, Route("api/bank")]
    public class BankController : ApiControllerBase
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

            return Content(banks);
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateBank([Bind(nameof(GuildBank.GuildId), nameof(GuildBank.Name))] GuildBank bank)
        {
            if (!await User.IsDiscordAdminAsync(bank.GuildId))
                return Forbid(ForbidReason.NotDiscordAdmin);

            _bank.Configure(bank.GuildId, User.GetId());

            try
            {
                var b = await _bank.CreateGuildBankAsync(bank.Name);
                return Content(b);
            }
            catch (UniqueIndexViolationException)
            {
                return BadRequest("A bank with the given name already exists.");
            }
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

    }
}