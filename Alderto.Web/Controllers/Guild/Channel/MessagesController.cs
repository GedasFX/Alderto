using System;
using System.Threading.Tasks;
using Alderto.Services;
using Alderto.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild.Channel
{
    [Route("api/guilds/{guildId}/channels/{channelId}/messages")]
    public class MessagesController : ApiControllerBase
    {
        private readonly IMessagesManager _msgManager;

        public MessagesController(IMessagesManager msgManager)
        {
            _msgManager = msgManager;
        }

        [HttpGet]
        public IActionResult List(ulong guildId, ulong channelId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{messageId}")]
        public IActionResult Get(ulong guildId, ulong channelId, ulong msgId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ulong guildId, ulong channelId, [Bind(nameof(ApiMessage.Contents))] ApiMessage message)
        {
            var msg = await _msgManager.PostMessageAsync(guildId, channelId, message.Contents);
            return Content(new ApiMessage(msg));
        }

        [HttpPatch("{messageId}")]
        public async Task<IActionResult> Update(ulong guildId, ulong channelId, ulong messageId, [Bind] string message)
        {
            await _msgManager.EditMessageAsync(guildId, channelId, messageId, message);
            return Ok();
        }

        [HttpDelete("{messageId}")]
        public IActionResult Delete(ulong guildId, ulong channelId, ulong messageId)
        {
            throw new NotImplementedException();
        }
    }
}