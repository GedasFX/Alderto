using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Alderto.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Alderto.Web.Models.Bank;
using Discord.WebSocket;

namespace Alderto.Web.Controllers
{
    [Route("api/guilds/{guildId}/banks")]
    public class BankController : ApiControllerBase
    {
        private readonly IGuildBankManager _bank;
        private readonly DiscordSocketClient _client;

        public BankController(IGuildBankManager bank, DiscordSocketClient client)
        {
            _bank = bank;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ListBanks(ulong guildId)
        {
            var user = _client.GetGuild(guildId).GetUser(User.GetId());
            bool ValidateModifyAccess(GuildBank bank) =>
                user.Roles.Any(r => r.Id == bank.ModeratorRoleId) || user.GuildPermissions.Administrator;

            var banks = await _bank.GetGuildBanksAsync(guildId, o => o.Include(b => b.Contents));
            var outBanks = banks.Select(b => new ApiGuildBank(b) { CanModify = ValidateModifyAccess(b) });
            return Content(outBanks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(ulong guildId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId))]
            GuildBank bank)
        {
            // Ensure user has admin rights 
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            if (await _bank.GetGuildBankAsync(guildId, bank.Name) != null)
                return BadRequest(ErrorMessages.BankNameAlreadyExists);

            var b = await _bank.CreateGuildBankAsync(guildId, User.GetId(), bank.Name, bank.LogChannelId);

            return Content(new ApiGuildBank(b) { CanModify = true });
        }

        [HttpPatch("{bankId}")]
        public async Task<IActionResult> EditBank(ulong guildId, int bankId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId))]
            GuildBank bank)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

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

        [HttpDelete("{bankId}")]
        public async Task<IActionResult> RemoveBank(ulong guildId, int bankId)
        {
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            await _bank.RemoveGuildBankAsync(guildId, bankId);
            return Ok();
        }
    }
}