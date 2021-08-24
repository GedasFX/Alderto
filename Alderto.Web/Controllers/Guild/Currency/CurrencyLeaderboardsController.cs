using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alderto.Application.Features.Currency.Dto;
using Alderto.Data;
using Alderto.Data.Models;
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
        private readonly AldertoDbContext _context;

        public CurrencyLeaderboardsController(IMapper mapper, AldertoDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [RequireGuildMember]
        [HttpGet]
        public async Task<List<CurrencyWalletDto>> List(ulong guildId, Guid currencyId, int page, int limit = 1)
        {
            var guild = HttpContext.GetDiscordGuild();

            limit = limit < 50 ? limit : 50;

            var query = _context.GuildMemberWallets.ListItems(guild.Id, currencyId);

            return await _mapper.ProjectTo<CurrencyWalletDto>(query
                    .OrderByDescending(q => q.Amount)
                    .Page(page, limit))
                .ToListAsync();
        }
    }
}
