using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank;
using Alderto.Application.Features.Bank.Dto;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Alderto.Domain.Exceptions;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers.Guild.Bank
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/banks/{bankId:int}/items")]
    public class BankItemsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly AldertoDbContext _context;

        public BankItemsController(IMediator mediator, IMapper mapper, AldertoDbContext context)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<BankItemDto>> ListBankItems(ulong guildId, int bankId)
        {
            return await _mapper.ProjectTo<BankItemDto>(_context.GuildBankItems.ListItems(guildId, bankId))
                .ToListAsync();
        }

        [HttpGet("{itemId:int}")]
        public async Task<BankItemDto> GetBankItem(ulong guildId, int bankId, int itemId)
        {
            var item = await _mapper.ProjectTo<BankItemDto>(_context.GuildBankItems.FindItem(guildId, bankId, itemId))
                .SingleOrDefaultAsync();
            if (item == null)
                throw new NotFoundDomainException(ErrorMessage.BANK_ITEM_NOT_FOUND);

            return item;
        }

        [HttpPost]
        [RequireGuildModerator]
        public async Task<BankItemDto> CreateBankItem(ulong guildId, int bankId,
            CreateBankItem.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.BankId = bankId;

            return _mapper.Map<BankItemDto>(await _mediator.Send(command));
        }

        [HttpPatch("{itemId:int}")]
        [RequireGuildModerator]
        public async Task<BankItemDto> EditBankItem(ulong guildId, int bankId, int itemId,
            UpdateBankItem.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.BankId = bankId;
            command.Id = itemId;

            return _mapper.Map<BankItemDto>(await _mediator.Send(command));
        }

        [HttpDelete("{itemId:int}")]
        [RequireGuildModerator]
        public async Task<GuildBankItem> RemoveBankItem(ulong guildId, int bankId, int itemId)
        {
            return await _mediator.Send(new DeleteBankItem.Command(guildId, User.GetId(), bankId, itemId));
        }
    }
}
