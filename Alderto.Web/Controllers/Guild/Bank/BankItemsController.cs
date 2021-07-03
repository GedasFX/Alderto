using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank;
using Alderto.Application.Features.Bank.Dto;
using Alderto.Application.Features.Bank.Query;
using Alderto.Data.Models.GuildBank;
using Alderto.Domain.Exceptions;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Alderto.Web.Controllers.Guild.Bank
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/banks/{bankId:int}/items")]
    public class BankItemsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public BankItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<BankItemDto>> ListBankItems(ulong guildId, int bankId)
        {
            return await _mediator.Send(new BankItems.List<BankItemDto>(guildId, User.GetId(), bankId));
        }

        [HttpGet("{itemId:int}")]
        public async Task<BankItemDto> GetBankItem(ulong guildId, int bankId, int itemId)
        {
            var bank = await _mediator.Send(new BankItems.Find<BankItemDto>(guildId, User.GetId(), bankId, itemId));
            if (bank == null)
                throw new NotFoundDomainException(ErrorMessage.BANK_ITEM_NOT_FOUND);

            return bank;
        }

        [HttpPost]
        [RequireGuildModerator]
        public async Task<GuildBankItem> CreateBankItem(ulong guildId, int bankId,
            CreateBankItem.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.BankId = bankId;

            return await _mediator.Send(command);
        }

        [HttpPatch("{itemId}")]
        [RequireGuildModerator]
        public async Task<GuildBankItem> EditBankItem(ulong guildId, int bankId, int itemId,
            UpdateBankItem.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.BankId = bankId;
            command.Id = itemId;

            return await _mediator.Send(command);
        }

        [HttpDelete("{itemId}")]
        [RequireGuildModerator]
        public async Task<GuildBankItem> RemoveBankItem(ulong guildId, int bankId, int itemId)
        {
            return await _mediator.Send(new DeleteBankItem.Command(guildId, User.GetId(), itemId));
        }
    }
}
