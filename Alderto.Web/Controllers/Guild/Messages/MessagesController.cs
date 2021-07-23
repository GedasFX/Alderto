using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.ManagedMessage;
using Alderto.Application.Features.ManagedMessage.Dto;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers.Guild.Messages
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/messages")]
    public class MessagesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly AldertoDbContext _context;

        public MessagesController(IMediator mediator, IMapper mapper, AldertoDbContext context)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IList<ManagedMessageDto>> ListMessages(ulong guildId)
        {
            return await _mapper.ProjectTo<ManagedMessageDto>(_context.GuildManagedMessages.ListItems(guildId))
                .ToListAsync();
        }

        [HttpGet("{messageId}")]
        public async Task<ManagedMessageDto?> GetMessage(ulong guildId, ulong messageId)
        {
            var bank = await _mapper
                .ProjectTo<ManagedMessageDto>(_context.GuildManagedMessages.FindItem(guildId, messageId))
                .SingleOrDefaultAsync();
            if (bank == null)
                throw new NotFoundDomainException(ErrorMessage.MANAGED_MESSAGE_NOT_FOUND);

            return bank;
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
