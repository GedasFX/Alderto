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
    [Route("guilds/{guildId}/banks")]
    public class BanksController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly AldertoDbContext _context;

        public BanksController(IMediator mediator, IMapper mapper, AldertoDbContext context)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IList<BankDto>> ListBanks(ulong guildId)
        {
            return await _mapper.ProjectTo<BankDto>(_context.GuildBanks.ListItems(guildId)).ToListAsync();
        }

        [HttpGet("{bankId:int}")]
        public async Task<BankDto?> GetBank(ulong guildId, int bankId)
        {
            var bank = await _mapper.ProjectTo<BankDto>(_context.GuildBanks.FindItem(guildId, bankId))
                .SingleOrDefaultAsync();
            if (bank == null)
                throw new NotFoundDomainException(ErrorMessage.BANK_NOT_FOUND);

            return bank;
        }

        [HttpPost]
        [RequireGuildAdmin]
        public async Task<GuildBank> CreateBank(ulong guildId,
            CreateBank.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();

            return await _mediator.Send(command);
        }

        [HttpPatch("{bankId:int}")]
        [RequireGuildAdmin]
        public async Task<GuildBank> EditBank(ulong guildId, int bankId,
            UpdateBank.Command command)
        {
            command.GuildId = guildId;
            command.MemberId = User.GetId();
            command.Id = bankId;

            return await _mediator.Send(command);
        }

        [HttpDelete("{bankId:int}")]
        [RequireGuildAdmin]
        public async Task<GuildBank> RemoveBank(ulong guildId, int bankId)
        {
            return await _mediator.Send(new DeleteBank.Command(guildId, User.GetId(), bankId));
        }
    }
}
