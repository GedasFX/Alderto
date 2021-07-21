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
    [Route("guilds/{guildId}/currencies/{currencyId:guid}/leaderboards")]
    public class CurrencyLeaderboardsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CurrencyWalletRepository _currencyWalletRepository;

        public CurrencyLeaderboardsController(IMapper mapper,
            CurrencyWalletRepository currencyWalletRepository)
        {
            _mapper = mapper;
            _currencyWalletRepository = currencyWalletRepository;
        }

        [RequireGuildMember]
        [HttpGet]
        public async Task<List<CurrencyWalletDto>> List(ulong guildId, Guid currencyId, int page, int limit = 1)
        {
            var guild = HttpContext.GetDiscordGuild();

            limit = limit < 50 ? limit : 50;

            var query = _currencyWalletRepository.List(guild.Id, currencyId);

            return await _mapper.ProjectTo<CurrencyWalletDto>(query
                    .OrderByDescending(q => q.Amount)
                    .Page(page, limit))
                .ToListAsync();
        }
    }
}
