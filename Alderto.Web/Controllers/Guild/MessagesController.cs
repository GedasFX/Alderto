using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.ManagedMessage;
using Alderto.Application.Features.ManagedMessage.Dto;
using Alderto.Application.Features.ManagedMessage.Query;
using Alderto.Data.Models;
using Alderto.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild
{
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
        public async Task<GuildManagedMessage> CreateMessage(ulong guildId,
            CreateMessage.Command command)
        {
            // if (!await _client.ValidateGuildAdminAsync(User.GetId(), guildId))
            //     throw new UserNotGuildAdminException();

            command.GuildId = guildId;
            command.MemberId = User.GetId();

            return await _mediator.Send(command);
        }

        [HttpPatch("{messageId}")]
        public async Task<GuildManagedMessage> EditMessage(ulong guildId, ulong messageId,
            UpdateMessage.Command command)
        {
            // if (message.ModeratorRoleId != null && !await _client.ValidateGuildAdminAsync(User.GetId(), guildId))
            //     throw new UserNotGuildAdminException();

            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.MessageId = messageId;

            return await _mediator.Send(command);
        }

        [HttpDelete("{messageId}")]
        public async Task<GuildManagedMessage> RemoveMessage(ulong guildId, ulong messageId)
        {
            // if (!await _client.ValidateGuildAdminAsync(User.GetId(), guildId))
            //     throw new UserNotGuildAdminException();

            return await _mediator.Send(new DeleteMessage.Command(guildId, User.GetId(), messageId));
        }
    }
}
