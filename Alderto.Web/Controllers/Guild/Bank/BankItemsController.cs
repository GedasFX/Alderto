using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services;
using Alderto.Services.Exceptions.NotFound;
using Alderto.Services.Exceptions.Forbid;
using Alderto.Web.Extensions;
using Alderto.Web.Models.Bank;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild.Bank
{
    [Route("guilds/{guildId}/banks/{bankId}/items")]
    public class BankItemsController : ApiControllerBase
    {
        private readonly IGuildBankItemsManager _items;
        private readonly IGuildBankManager _bank;
        private readonly IDiscordClient _client;

        public BankItemsController(IGuildBankItemsManager items, IGuildBankManager bank, IDiscordClient client)
        {
            _items = items;
            _bank = bank;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ListBankItems(ulong guildId, int bankId)
        {
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            if (bank == null)
                throw new BankNotFoundException();

            var items = await _items.GetBankItemsAsync(bank);
            return Content(items.Select(o => new ApiGuildBankItem(o)));
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetBankItem(ulong guildId, int bankId, int itemId)
        {
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            if (bank == null)
                throw new BankNotFoundException();

            var item = await _items.GetBankItemAsync(bank, itemId);
            if (item == null)
                throw new BankItemNotFoundException();

            return Content(new ApiGuildBankItem(item));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBankItem(ulong guildId, int bankId,
            [Bind(nameof(GuildBankItem.Name), nameof(GuildBankItem.Description), nameof(GuildBankItem.Value),
                nameof(GuildBankItem.ImageUrl), nameof(GuildBankItem.Quantity))]
            GuildBankItem item)
        {
            var userId = User.GetId();

            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            if (bank == null)
                throw new BankNotFoundException();

            if (!await ValidateWriteAccess(bank, userId))
                throw new UserNotGuildModeratorException();

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
            if (bank == null)
                throw new BankNotFoundException();

            if (!await ValidateWriteAccess(bank, userId))
                throw new UserNotGuildModeratorException();

            await _items.UpdateBankItemAsync(bank, itemId, userId, i =>
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
            if (bank == null)
                throw new BankNotFoundException();

            if (!await ValidateWriteAccess(bank, userId))
                throw new UserNotGuildModeratorException();

            await _items.RemoveBankItemAsync(bank, itemId, userId);
            return NoContent();
        }

        /// <summary>
        /// Validates if the user has access write access to the guild bank.
        /// </summary>
        /// <param name="bank">Bank to check access of.</param>
        /// <param name="userId">Id of user.</param>
        /// <returns>Corresponding HTTP error result. null if user has write access.</returns>
        private async Task<bool> ValidateWriteAccess(GuildBank bank, ulong userId)
        {
            // Get the guild and check if it is present.
            var guild = await _client.GetGuildAsync(bank.GuildId);
            if (guild == null)
                throw new GuildNotFoundException();

            // Check if user even exists in the guild.
            var user = await guild.GetUserAsync(userId);
            if (user == null)
                throw new UserNotFoundException();

            // Check if user has write access to the bank.
            return user.RoleIds.Any(r => r == bank.ModeratorRoleId) || user.GuildPermissions.Administrator;
        }
    }
}