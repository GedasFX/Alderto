using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.Currency;
using Alderto.Application.Features.Currency.Dto;
using Alderto.Application.Repository;
using Alderto.Domain.Exceptions;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers.Guild.Currency
{
    [RequireGuildMember]
    [Route("guilds/{guildId}/currencies")]
    public class CurrenciesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly CurrencyRepository _currencyRepository;

        public CurrenciesController(IMediator mediator, CurrencyRepository currencyRepository)
        {
            _mediator = mediator;
            _currencyRepository = currencyRepository;
        }

        [HttpGet]
        public async Task<IList<CurrencyDto>> ListCurrencies(ulong guildId)
        {
            return await _currencyRepository.List<CurrencyDto>(guildId).ToListAsync();
        }

        [HttpGet("{currencyId:guid}")]
        public async Task<CurrencyDto?> Get(ulong guildId, Guid currencyId)
        {
            var currency = await _currencyRepository.Find<CurrencyDto>(guildId, currencyId).SingleOrDefaultAsync();
            if (currency == null)
                throw new NotFoundDomainException(ErrorMessage.CURRENCY_NOT_FOUND);

            return currency;
        }

        [HttpPost]
        [RequireGuildAdmin]
        public async Task<Data.Models.Currency> Post(ulong guildId,
            CreateCurrency.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();

            return await _mediator.Send(command);
        }

        [HttpPatch("{currencyId:guid}")]
        [RequireGuildAdmin]
        public async Task<Data.Models.Currency> Patch(ulong guildId, Guid currencyId,
            UpdateCurrency.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.Id = currencyId;

            return await _mediator.Send(command);
        }

        [HttpDelete("{currencyId:guid}")]
        [RequireGuildAdmin]
        public async Task<Data.Models.Currency> Delete(ulong guildId, Guid currencyId)
        {
            return await _mediator.Send(new DeleteCurrency.Command(guildId, User.GetId(), currencyId));
        }
    }
}
