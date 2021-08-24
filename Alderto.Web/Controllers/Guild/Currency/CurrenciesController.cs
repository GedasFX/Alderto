using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alderto.Application.Features.Currency;
using Alderto.Application.Features.Currency.Dto;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly AldertoDbContext _context;

        public CurrenciesController(IMediator mediator, IMapper mapper, AldertoDbContext context)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IList<CurrencyDto>> ListCurrencies(ulong guildId)
        {
            return await _mapper.ProjectTo<CurrencyDto>(_context.Currencies.ListItems(guildId)).ToListAsync();
        }

        [HttpGet("{currencyId:guid}")]
        public async Task<CurrencyDto?> Get(ulong guildId, Guid currencyId)
        {
            var currency = await _mapper.ProjectTo<CurrencyDto>(_context.Currencies.FindItem(guildId, currencyId))
                .SingleOrDefaultAsync();
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
