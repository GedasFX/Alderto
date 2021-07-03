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
    public static class DeleteBankItem
    {
        public class Command : CommandRequest<GuildBankItem>
        {
            [Range(1, int.MaxValue)]
            public int Id { get; }

            [Range(1, int.MaxValue)]
            public int BankId { get; set; }

            public Command(ulong guildId, ulong memberId, int id) : base(guildId, memberId)
            {
                Id = id;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildBankItem>
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

            private IQueryable<GuildBankItem> GuildBankItems(ulong guildId, int bankId) =>
                _context.GuildBankItems.Include(i => i.GuildBank)
                    .Where(i => i.GuildBank!.GuildId == guildId && i.GuildBankId == bankId);

            public async Task<GuildBankItem> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = await GuildBankItems(request.GuildId, request.BankId)
                    .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);

                if (item == null)
                    throw new ValidationDomainException(ErrorMessage.BANK_ITEM_NOT_FOUND);

                _context.GuildBankItems.Remove(item);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.Log(LogLevel.Information, 420, "{GuildId}__{User} has removed '{ItemName}' the '{BankName}' bank",
                    request.GuildId, MentionUtils.MentionUser(request.MemberId), item.Name, item.GuildBank!.Name);

                return item;
            }
        }
    }
}
