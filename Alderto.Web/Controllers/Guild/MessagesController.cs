using System.Linq;
using System.Threading.Tasks;
using Alderto.Services;
using Alderto.Services.Exceptions;
using Alderto.Web.Extensions;
using Alderto.Web.Models;
using Discord;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
    [Route("guilds/{guildId}/messages")]
    public class MessagesController : ApiControllerBase
    {
        private readonly IMessagesManager _msgManager;
        private readonly IDiscordClient _client;

        public MessagesController(IMessagesManager msgManager, IDiscordClient client)
        {
            _msgManager = msgManager;
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ListMessages(ulong guildId)
        {
            var messages = await _msgManager.ListMessagesAsync(guildId);
            return Content(messages.Select(g => new ApiManagedMessage(g)));
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetMessage(ulong guildId, ulong messageId)
        {
            var msg = await _msgManager.GetMessageAsync(guildId, messageId);
            if (msg == null)
                throw new MessageNotFoundException();

            return Content(new ApiManagedMessage(msg));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(ulong guildId,
            [Bind(nameof(ApiManagedMessage.Content), nameof(ApiManagedMessage.ChannelId), nameof(ApiManagedMessage.Id), nameof(ApiManagedMessage.ModeratorRoleId))]
            ApiManagedMessage message)
        {
            if (!await _client.ValidateGuildAdminAsync(User.GetId(), guildId))
                throw new UserNotGuildAdminException();

            // If create new message
            if (message.Content != null)
            {
                var msg = await _msgManager.PostMessageAsync(guildId, message.ChannelId, message.Content);
                return Content(new ApiManagedMessage(msg));
            }

            // If import a message
            if (message.Id != 0)
            {
                var msg = await _msgManager.ImportMessageAsync(guildId, message.ChannelId, message.Id);
                return Content(new ApiManagedMessage(msg));
            }

            return BadRequest();
        }

        [HttpPatch("{messageId}")]
        public async Task<IActionResult> EditMessage(ulong guildId, ulong messageId,
            [Bind(nameof(ApiManagedMessage.Content), nameof(ApiManagedMessage.ModeratorRoleId))]
            ApiManagedMessage message)
        {
            if (message.ModeratorRoleId != null && !await _client.ValidateGuildAdminAsync(User.GetId(), guildId))
                throw new UserNotGuildAdminException();

            var dbMsg = await _msgManager.GetMessageAsync(guildId, messageId);
            if (dbMsg == null)
                throw new MessageNotFoundException();

            if (!await _client.ValidateResourceModeratorAsync(User.GetId(), guildId, dbMsg.ModeratorRoleId))
                throw new UserNotGuildModeratorException();

            await _msgManager.EditMessageAsync(guildId, messageId, message.Content, message.ModeratorRoleId);
            return Ok();
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> RemoveMessage(ulong guildId, ulong messageId)
        {
            await _msgManager.RemoveMessageAsync(guildId, messageId);
            return NoContent();
        }
    }
}