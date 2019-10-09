using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Services.Exceptions;
using Alderto.Web.Extensions;
using Alderto.Web.Models.Bank;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers.Guild.Bank
{
    [Route("guilds/{guildId}/banks")]
    public class BanksController : ApiControllerBase
    {
        private readonly IGuildBankManager _bank;
        private readonly DiscordSocketClient _client;

        public BanksController(IGuildBankManager bank, DiscordSocketClient client)
        {
            _bank = bank;
            _client = client;
        }

#pragma warning disable CA1062 // Validate arguments of public methods

        [HttpGet]
        public async Task<IActionResult> ListBanks(ulong guildId)
        {
            var user = _client.GetGuild(guildId)?.GetUser(User.GetId());
            if (user == null)
                return NotFound();

            bool ValidateModifyAccess(GuildBank bank) =>
                user.Roles.Any(r => r.Id == bank.ModeratorRoleId) || user.GuildPermissions.Administrator;

            var banks = await _bank.GetGuildBanksAsync(guildId, o => o.Include(b => b.Contents));
            var outBanks = banks.Select(b => new ApiGuildBank(b) { CanModify = ValidateModifyAccess(b) });
            return Content(outBanks);
        }

        [HttpGet("{bankId}")]
        public IActionResult GetBank(ulong guildId, int bankId)
        {
            return Content("Placeholder");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBank(ulong guildId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId), nameof(GuildBank.ModeratorRoleId))]
            GuildBank bank)
        {
            // Ensure user has admin rights 
            if (!_client.ValidateGuildAdmin(User.GetId(), guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);


            if (await _bank.GetGuildBankAsync(guildId, bank!.Name) != null)
                return BadRequest(ErrorMessages.BankNameAlreadyExists);

            try
            {
                var b = await _bank.CreateGuildBankAsync(guildId, User.GetId(), bank);
                return Content(new ApiGuildBank(b) { CanModify = true });
            }
            catch (HttpException e)
            {
                return HandleHttpError(e);
            }
        }

        [HttpPatch("{bankId}")]
        public async Task<IActionResult> EditBank(ulong guildId, int bankId,
            [Bind(nameof(GuildBank.Name), nameof(GuildBank.LogChannelId), nameof(GuildBank.ModeratorRoleId))]
            GuildBank bank)
        {
            if (!User.IsDiscordAdminAsync(_client, guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            // If not renaming this would always return itself. Check for id difference instead.
            var dbBank = await _bank.GetGuildBankAsync(guildId, bank.Name);
            if (dbBank != null && dbBank.Id != bankId)
                return BadRequest(ErrorMessages.BankNameAlreadyExists);

            try
            {
                await _bank.UpdateGuildBankAsync(guildId, bankId, User.GetId(), b =>
                {
                    b.Name = bank.Name;
                    b.LogChannelId = bank.LogChannelId;
                    b.ModeratorRoleId = bank.ModeratorRoleId;
                });
            }
            catch (HttpException e)
            {
                return HandleHttpError(e);
            }

            return NoContent();
        }

        [HttpDelete("{bankId}")]
        public async Task<IActionResult> RemoveBank(ulong guildId, int bankId)
        {
            if (!User.IsDiscordAdminAsync(_client, guildId))
                return Forbid(ErrorMessages.UserNotDiscordAdmin);

            try
            {
                await _bank.RemoveGuildBankAsync(guildId, bankId, User.GetId());
            }
            catch (HttpException e)
            {
                return HandleHttpError(e);
            }

            return NoContent();
        }

        /// <summary>
        /// Handles MissingAccess and MissingPermissions discord error codes.
        /// </summary>
        private IActionResult HandleHttpError(HttpException e)
        {
            return e.DiscordCode switch
            {
                50001 => BadRequest(ErrorMessages.MissingChannelAccess),
                50013 => BadRequest(ErrorMessages.MissingWritePermissions),
                _ => throw e
            };
        }
    }
}