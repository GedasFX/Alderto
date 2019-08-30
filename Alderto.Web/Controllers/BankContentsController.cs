using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Alderto.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers
{
    [Route("api/guild/{guildId}/bank/contents")]
    public class BankContentsController : ApiControllerBase
    {
        private readonly IGuildBankItemManager _bankContents;

        public BankContentsController(IGuildBankItemManager bankContents)
        {
            _bankContents = bankContents;
        }

        [HttpGet, Route("list/{guildId}")]
        public async Task<IActionResult> ListItems(ulong guildId)
        {
            var banks = await _bankContents.GetAllGuildBanksAsync(guildId, o => o.Include(b => b.Contents));

            return Content(banks);
        }

        [HttpPost, Route("create/{guildId}")]
        public async Task<IActionResult> CreateBank(ulong guildId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId))]
            GuildBank bank)
        {
            // Ensure user has admin rights
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.NotDiscordAdmin);

            if (await _bank.GetGuildBankAsync(guildId, bank.Name) != null)
                return BadRequest(ErrorMessages.BankNameAlreadyExists);

            var b = await _bank.CreateGuildBankAsync(guildId, bank.Name, bank.LogChannelId);

            // Ensure guild is null to prevent serializer loop.
            b.Guild = null;

            return Content(b);
        }

        [HttpPatch, Route("edit/{guildId}/{bankId}")]
        public async Task<IActionResult> EditBank(ulong guildId, int bankId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId))]
            GuildBank bank)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.NotDiscordAdmin);

            // If not renaming this would always return itself. Check for id difference instead.
            var dbBank = await _bank.GetGuildBankAsync(guildId, bank.Name);
            if (dbBank != null && dbBank.Id != bankId)
                return BadRequest(ErrorMessages.BankNameAlreadyExists);

            await _bank.UpdateGuildBankAsync(guildId, bankId, b =>
            {
                b.Name = bank.Name;
                b.LogChannelId = bank.LogChannelId;
            });

            return Ok();
        }

        [HttpDelete, Route("remove/{guildId}/{bankId}")]
        public async Task<IActionResult> RemoveBank(ulong guildId, int bankId)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.NotDiscordAdmin);

            await _bank.RemoveGuildBankAsync(guildId, bankId);
            return Ok();
        }
    }
}