using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Services.Exceptions;
using Alderto.Web.Extensions;
using Alderto.Web.Models.Bank;
using Discord;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers.Guild.Bank
{
    [Route("guilds/{guildId}/banks")]
    public class BanksController : ApiControllerBase
    {
        private readonly IGuildBankManager _bank;
        private readonly IDiscordClient _client;

        public BanksController(IGuildBankManager bank, IDiscordClient client)
        {
            _bank = bank;
            _client = client;
        }

#pragma warning disable CA1062 // Validate arguments of public methods

        [HttpGet]
        public async Task<IActionResult> ListBanks(ulong guildId)
        {
            var guild = await _client.GetGuildAsync(guildId);
            if (guild == null)
                throw new GuildNotFoundException();

            var user = await guild.GetUserAsync(User.GetId());
            if (user == null)
                throw new UserNotFoundException();

            var banks = await _bank.GetGuildBanksAsync(guildId, o => o.Include(b => b.Contents));
            var outBanks = banks.Select(b => new ApiGuildBank(b) { CanModify = ValidateModifyAccess(b, user) });

            return Content(outBanks);
        }

        [HttpGet("{bankId}")]
        public async Task<IActionResult> GetBank(ulong guildId, int bankId)
        {
            // Check if user even exists in the guild.
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            if (bank == null)
                throw new BankNotFoundException();

            return Content(new ApiGuildBank(bank));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(ulong guildId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId), nameof(GuildBank.ModeratorRoleId))]
            GuildBank bank)
        {
            var userId = User.GetId();

            // Ensure user has admin rights 
            if (!await _client.ValidateGuildAdmin(userId, guildId))
                throw new UserNotGuildAdminException();

            if (await _bank.GetGuildBankAsync(guildId, bank!.Name) != null)
                throw new BankNameAlreadyExistsException();

            var b = await _bank.CreateGuildBankAsync(guildId, userId, bank);

            return Content(new ApiGuildBank(b) { CanModify = true });
        }

        [HttpPatch("{bankId}")]
        public async Task<IActionResult> EditBank(ulong guildId, int bankId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId), nameof(GuildBank.ModeratorRoleId))]
            GuildBank bank)
        {
            var userId = User.GetId();

            // Ensure user has admin rights 
            if (!await _client.ValidateGuildAdmin(userId, guildId))
                throw new UserNotGuildAdminException();

            // If not renaming this would always return itself. Check for id difference instead.
            var dbBank = await _bank.GetGuildBankAsync(guildId, bank.Name);
            if (dbBank != null && dbBank.Id != bankId)
                throw new BankNameAlreadyExistsException();

            await _bank.UpdateGuildBankAsync(guildId, bankId, userId, b =>
            {
                b.Name = bank.Name;
                b.LogChannelId = bank.LogChannelId;
                b.ModeratorRoleId = bank.ModeratorRoleId;
            });

            return Ok();
        }

        [HttpDelete("{bankId}")]
        public async Task<IActionResult> RemoveBank(ulong guildId, int bankId)
        {
            var userId = User.GetId();

            // Ensure user has admin rights 
            if (!await _client.ValidateGuildAdmin(userId, guildId))
                throw new UserNotGuildAdminException();

            await _bank.RemoveGuildBankAsync(guildId, bankId, userId);

            return NoContent();
        }

        private static bool ValidateModifyAccess(GuildBank bank, IGuildUser user) =>
            user.RoleIds.Any(r => r == bank.ModeratorRoleId) || user.GuildPermissions.Administrator;
    }
}