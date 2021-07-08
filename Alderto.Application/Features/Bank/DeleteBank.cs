using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Data.Models.GuildBank;
using Alderto.Domain.Exceptions;
using AutoMapper;
using Discord;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Alderto.Application.Features.Bank
{
    public static class DeleteBank
    {
        public class Command : CommandRequest<GuildBank>
        {
            public int? Id { get; }

            [MaxLength(32)]
            public string? Name { get; }

            public Command(ulong guildId, ulong memberId, int id) : base(guildId, memberId)
            {
                Id = id;
            }

            public Command(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildBank>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(AldertoDbContext context, IMapper mapper, ILogger<CommandHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            private IQueryable<GuildBank> GuildBanks(ulong guildId) =>
                _context.GuildBanks.AsQueryable().Where(b => b.GuildId == guildId);

            public async Task<GuildBank> Handle(Command request, CancellationToken cancellationToken)
            {
                GuildBank? bank;
                if (request.Id != null)
                    bank = await GuildBanks(request.GuildId)
                        .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken: cancellationToken);
                else if (request.Name != null)
                    bank = await GuildBanks(request.GuildId)
                        .SingleOrDefaultAsync(b => b.Name == request.Name, cancellationToken: cancellationToken);
                else
                    bank = null;

                if (bank == null)
                    throw new ValidationDomainException(ErrorMessage.BANK_NOT_FOUND);

                _context.GuildBanks.Remove(bank);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.Log(LogLevel.Information, 420, "{GuildId}__{User} has deleted the '{BankName}' bank",
                    request.GuildId, MentionUtils.MentionUser(request.MemberId), request.Name);

                return bank;
            }
        }
    }
}
