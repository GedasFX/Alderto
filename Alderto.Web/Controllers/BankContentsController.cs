using System.Linq;
using System.Threading.Tasks;
using Alderto.Data.Models.GuildBank;
using Alderto.Services.GuildBankManagers;
using Alderto.Web.Extensions;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("api/guilds/{guildId}/banks/{bankId}")]
    public class BankContentsController : ApiControllerBase
    {
        private readonly IGuildBankItemManager _contents;
        private readonly IGuildBankManager _bank;
        private readonly DiscordSocketClient _client;

        public BankContentsController(IGuildBankItemManager contents, IGuildBankManager bank, DiscordSocketClient client)
        {
            _contents = contents;
            _bank = bank;
            _client = client;
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateItem(ulong guildId, int bankId,
            [Bind(nameof(GuildBankItem.Name), nameof(GuildBankItem.Description), nameof(GuildBankItem.Value),
                nameof(GuildBankItem.ImageUrl), nameof(GuildBankItem.Quantity))]
            GuildBankItem item)
        {
            var errorResult = await ValidateWriteAccess(guildId, bankId);
            if (errorResult != null)
                return errorResult;

            var createdBank = await _contents.CreateBankItemAsync(bankId, item);

            return Content(createdBank);
        }

        [HttpPatch("items/{itemId}")]
        public async Task<IActionResult> EditItem(ulong guildId, int bankId, int itemId,
            [Bind(nameof(GuildBankItem.Name), nameof(GuildBankItem.Description), nameof(GuildBankItem.Value),
                nameof(GuildBankItem.ImageUrl), nameof(GuildBankItem.Quantity))]
            GuildBankItem item)
        {
            var errorResult = await ValidateWriteAccess(guildId, bankId);
            if (errorResult != null)
                return errorResult;

            await _contents.UpdateBankItemAsync(itemId, i =>
            {
                i.Name = item.Name;
                i.Description = item.Description;
                i.Value = item.Value;
                i.ImageUrl = item.ImageUrl;
                i.Quantity = item.Quantity;
            });

            return Ok();
        }

        /// <summary>
        /// Validates if the user has access write access to the guild bank.
        /// </summary>
        /// <param name="guildId">Id of guild</param>
        /// <param name="bankId">Id of bank</param>
        /// <returns>Corresponding HTTP error result. null if user has write access.</returns>
        private async Task<IActionResult> ValidateWriteAccess(ulong guildId, int bankId)
        {
            // First get the bank.
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            if (bank == null)
                return NotFound(ErrorMessages.BankNotFound);

            var userId = User.GetId();

            // Get the guild and check if it is present.
            var guild = _client.GetGuild(guildId);
            if (guild == null)
                return NotFound(ErrorMessages.GuildNotFound);

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