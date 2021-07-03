using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.ManagedMessage;
using Alderto.Application.Features.ManagedMessage.Dto;
using Alderto.Application.Features.ManagedMessage.Query;
using Alderto.Data.Models;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/messages")]
    public class MessagesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IList<ManagedMessageDto>> ListMessages(ulong guildId)
        {
            return await _mediator.Send(new Messages.List<ManagedMessageDto>(guildId, User.GetId()));
        }

        [HttpGet("{messageId}")]
        public async Task<ManagedMessageDto?> GetMessage(ulong guildId, ulong messageId)
        {
            return await _mediator.Send(new Messages.Find<ManagedMessageDto>(guildId, User.GetId(), messageId));
        }

        [HttpPost]
        [RequireGuildAdmin]
        public async Task<GuildManagedMessage> CreateMessage(ulong guildId,
            CreateMessage.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();

            return await _mediator.Send(command);
        }

        [HttpPatch("{messageId}")]
        [RequireGuildModerator]
        public async Task<GuildManagedMessage> EditMessage(ulong guildId, ulong messageId,
            UpdateMessage.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.MessageId = messageId;

            return await _mediator.Send(command);
        }

        [HttpDelete("{messageId}")]
        [RequireGuildAdmin]
        public async Task<GuildManagedMessage> RemoveMessage(ulong guildId, ulong messageId)
        {
            return await _mediator.Send(new DeleteMessage.Command(guildId, User.GetId(), messageId));
        }
    }
}
