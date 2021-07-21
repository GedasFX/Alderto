using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Application.Features.Currency.Dto;
using Alderto.Application.Repository;
using Alderto.Data;
using Alderto.Web.Attributes;
using Alderto.Web.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Web.Controllers.Guild.Currency
{
    [Route("guilds/{guildId}/currencies/{currencyId:guid}/transactions")]
    public class CurrencyTransactionsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CurrencyTransactionRepository _currencyTransactionRepository;

        public CurrencyTransactionsController(IMapper mapper,
            CurrencyTransactionRepository currencyTransactionRepository)
        {
            _mapper = mapper;
            _currencyTransactionRepository = currencyTransactionRepository;
        }

        [RequireGuildMember]
        [HttpGet("@me")]
        public Task<List<CurrencyTransactionDto>> List(ulong guildId, Guid currencyId, int page, int limit = 1)
        {
            return List(guildId, currencyId, User.GetId(), page, limit);
        }

        [RequireGuildMember]
        [HttpGet("{userId}")]
        public async Task<List<CurrencyTransactionDto>> List(ulong guildId, Guid currencyId, ulong userId,
            int page, int limit = 1)
        {
            var guild = HttpContext.GetDiscordGuild();

            limit = limit < 50 ? limit : 50;

            var query = _currencyTransactionRepository.List(guild.Id, currencyId, userId);

            return await _mapper.ProjectTo<CurrencyTransactionDto>(query
                    .OrderByDescending(q => q.Id)
                    .Page(page, limit))
                .ToListAsync();
        }
    }
}
