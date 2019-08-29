using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Alderto.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers
{
    [Route("api/bank")]
    public class BankController : ApiControllerBase
    {
        private readonly IGuildBankManager _bank;

        public BankController(IGuildBankManager bank)
        {
            _bank = bank;
        }

        [HttpGet, Route("list/{guildId}")]
        public async Task<IActionResult> ListBanks(ulong guildId)
        {
            var banks = await _bank.GetAllGuildBanksAsync(guildId, o => o.Include(b => b.Contents));

            return Content(banks);
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateBank(
            [Bind(nameof(GuildBank.GuildId), nameof(GuildBank.Name), nameof(GuildBank.LogChannelId))]
            GuildBank bank)
        {
            // Ensure user has admin rights
            if (!await User.IsDiscordAdminAsync(bank.GuildId))
                return Forbid(ForbidReason.NotDiscordAdmin);
            
            if (await _bank.GetGuildBankAsync(bank.GuildId, bank.Name) != null)
                return BadRequest("A bank with the given name already exists.");

            var b = await _bank.CreateGuildBankAsync(bank.GuildId, bank.Name, bank.LogChannelId);

            // Ensure guild is null to prevent serializer loop.
            b.Guild = null;

            return Content(b);
        }


        [HttpPatch, Route("rename")]
        public async Task<IActionResult> RenameBank(ulong guildId, int bankId, string newName)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ForbidReason.NotDiscordAdmin);

            await _bank.UpdateGuildBankAsync(guildId, bankId, b => { b.Name = newName; });

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBank(ulong guildId, int bankId)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ForbidReason.NotDiscordAdmin);

            await _bank.RemoveGuildBankAsync(guildId, bankId);
            return Ok();
        }

    }
}