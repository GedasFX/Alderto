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
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.NotDiscordAdmin);

            var bank = _bank.GetGuildBankAsync(guildId, bankId);
            if (bank == null)
                return BadRequest(ErrorMessages.BankDoesNotExist);

            var i = await _contents.CreateBankItemAsync(bankId, item);

            return Content(i);
        }
    }
}