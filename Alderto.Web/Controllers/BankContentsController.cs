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

            var createdBank = await _contents.CreateBankItemAsync(bankId, item);

            return Content(createdBank);
        }
    }
}