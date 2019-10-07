using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Services.Exceptions;
using Alderto.Services.Exceptions.NotFound;
using Alderto.Web.Extensions;
using Alderto.Web.Models.Bank;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers.Guild.Bank
{
    [Route("guilds/{guildId}/banks/{bankId}/items")]
    public class BankItemsController : ApiControllerBase
    {
        private readonly IGuildBankItemsManager _items;
        private readonly IGuildBankManager _bank;
        private readonly DiscordSocketClient _client;

        public BankItemsController(IGuildBankItemsManager items, IGuildBankManager bank, DiscordSocketClient client)
        {
            _items = items;
            _bank = bank;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ListBankItems(ulong guildId, int bankId)
        {
            var userId = User.GetId();
            var bank = await _bank.GetGuildBankAsync(guildId, bankId, o => o.Include(prop => prop.Contents));
            return Content(new ApiGuildBank(bank).Contents);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetBankItem(ulong guildId, int bankId, int itemId)
        {
            return Content("Placeholder");
            var userId = User.GetId();
            var bank = await _bank.GetGuildBankAsync(guildId, bankId, o => o.Include(prop => prop.Contents));
            return Content(new ApiGuildBank(bank).Contents);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBankItem(ulong guildId, int bankId,
            [Bind(nameof(GuildBankItem.Name), nameof(GuildBankItem.Description), nameof(GuildBankItem.Value),
                nameof(GuildBankItem.ImageUrl), nameof(GuildBankItem.Quantity))]
            GuildBankItem item)
        {
            var userId = User.GetId();
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);

            var errorResult = ValidateWriteAccess(bank, userId);
            if (errorResult != null)
                return errorResult;

            var createdBank = await _items.CreateBankItemAsync(bank, item, userId);

            return Content(new ApiGuildBankItem(createdBank));
        }

        [HttpPatch("{itemId}")]
        public async Task<IActionResult> EditBankItem(ulong guildId, int bankId, int itemId,
            [Bind(nameof(GuildBankItem.Name), nameof(GuildBankItem.Description), nameof(GuildBankItem.Value),
                nameof(GuildBankItem.ImageUrl), nameof(GuildBankItem.Quantity))]
            GuildBankItem item)
        {
            var userId = User.GetId();
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            var errorResult = ValidateWriteAccess(bank, userId);
            if (errorResult != null)
                return errorResult;

            await _items.UpdateBankItemAsync(itemId, userId, i =>
            {
                i.Name = item.Name;
                i.Description = item.Description;
                i.Value = item.Value;
                i.ImageUrl = item.ImageUrl;
                i.Quantity = item.Quantity;
            });

            return Ok();
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> RemoveBankItem(ulong guildId, int bankId, int itemId)
        {
            var userId = User.GetId();
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            var errorResult = ValidateWriteAccess(bank, userId);
            if (errorResult != null)
                return errorResult;

            await _items.RemoveBankItemAsync(itemId, userId);
            return Ok();
        }

        /// <summary>
        /// Validates if the user has access write access to the guild bank.
        /// </summary>
        /// <param name="bank">Bank to check access of.</param>
        /// <param name="userId">Id of user.</param>
        /// <returns>Corresponding HTTP error result. null if user has write access.</returns>
        private IActionResult ValidateWriteAccess(GuildBank bank, ulong userId)
        {
            // First get the bank.
            if (bank == null)
                throw new BankNotFoundException();

            // Get the guild and check if it is present.
            var guild = _client.GetGuild(bank.GuildId);
            if (guild == null)
                throw new GuildNotFoundException();

            // Check if user even exists in the guild.
            var user = guild.GetUser(userId);
            if (user == null)
                return NotFound(ErrorMessages.UserNotFound);

            // Check if user has write access to the bank.
            if (!(user.Roles.Any(r => r.Id == bank.ModeratorRoleId) || user.GuildPermissions.Administrator))
                return Forbid(ErrorMessages.UserNotBankModerator);

            // User has write access to the guild. Continue.
            return null;
        }
    }
}