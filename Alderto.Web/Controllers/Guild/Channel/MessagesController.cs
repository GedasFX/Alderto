using System.Threading.Tasks;
using Alderto.Services;
using Alderto.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild.Channel
{
    [Route("guilds/{guildId}/messages")]
    public class MessagesController : ApiControllerBase
    {
        private readonly IMessagesManager _msgManager;

        public MessagesController(IMessagesManager msgManager)
        {
            _msgManager = msgManager;
        }

        [HttpGet]
        public async Task<IActionResult> ListMessages(ulong guildId)
        {
            var messages = await _msgManager.ListMessagesAsync(guildId);
            return Content(messages);
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetMessage(ulong guildId, ulong messageId)
        {
            var msg = await _msgManager.GetMessageAsync(guildId, messageId);
            return Content(new ApiMessage(msg));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(ulong guildId,
            [Bind(nameof(ApiMessage.Contents), nameof(ApiMessage.ChannelId), nameof(ApiMessage.Id))]
            ApiMessage message)
        {
            // If create new message
            if (message.Contents != null)
            {
                var msg = await _msgManager.PostMessageAsync(guildId, message.ChannelId, message.Contents);
                return Content(new ApiMessage(msg));
            }

            // If import a message
            if (message.Id != 0)
            {
                var msg = await _msgManager.ImportMessageAsync(guildId, message.ChannelId, message.Id);
                return Content(new ApiMessage(msg));
            }

            return BadRequest();
        }

        [HttpPatch("{messageId}")]
        public async Task<IActionResult> EditMessage(ulong guildId, ulong messageId, [Bind(nameof(ApiMessage.Contents))] ApiMessage message)
        {
            await _msgManager.EditMessageAsync(guildId, messageId, message.Contents);
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