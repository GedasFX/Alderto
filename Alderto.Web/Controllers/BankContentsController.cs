using System.Linq;
using System.Threading.Tasks;
using Alderto.Services.GuildBankManagers;
using Alderto.Web.Extensions;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers
{
    [Route("api/guilds/{guildId}/banks/{bankId}")]
    public class BankContentsController : ApiControllerBase
    {
        private readonly IGuildBankItemManager _bankContents;
        private readonly IGuildBankManager _bank;
        private readonly DiscordSocketClient _client;

        public BankContentsController(IGuildBankItemManager bankContents, IGuildBankManager bank, DiscordSocketClient client)
        {
            _bankContents = bankContents;
            _bank = bank;
            _client = client;
        }

        [HttpGet("contents")]
        public async Task<IActionResult> Contents(ulong guildId, int bankId)
        {
            // TODO: Add private banks.

            var bank = await _bank.GetGuildBankAsync(guildId, bankId);

            // Check if bank viewing is not open to the public.
            if (bank.ViewerRoleId != null)
            {
                var user = _client.GetGuild(guildId).GetUser(User.GetId());
                if (!user.Roles.Any(r => r.Id == bank.ViewerRoleId || r.Id == bank.ModeratorRoleId));
            }

            return Content(_bankContents.GetGuildBankItems(bankId));
        }

        [HttpPost("contents")]
        public async Task<IActionResult> AddContent(ulong guildId, int bankId)
        {
            var bank = await _bank.GetGuildBankAsync(guildId, bankId);
            //var any = _client.GetGuild(guildId).GetUser(User.GetId()).GuildPermissions.Administrator.Roles.Any(r => r.Id == bank.ModeratorRoleId);
            if (!await User.IsDiscordAdminAsync(guildId))
                return Forbid(ErrorMessages.NotDiscordAdmin);
            return Content(_bankContents.GetGuildBankItems(bankId));
        }

    }
}