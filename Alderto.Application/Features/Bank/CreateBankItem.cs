using System;
using System.ComponentModel.DataAnnotations;
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
    public static class CreateBankItem
    {
        public class Command : CommandRequest<GuildBankItem>
        {
            [Range(1, int.MaxValue)]
            public int BankId { get; set; }

            [MaxLength(70), Required]
            public string Name { get; set; }

            public double Value { get; set; }

            public double Quantity { get; set; }

            [MaxLength(280)]
            public string? Description { get; set; }

            [MaxLength(140)]
            public string? ImageUrl { get; set; }

            public Command(ulong guildId, ulong memberId, int bankId, string name, double value = 0,
                double quantity = 0, string? description = null, string? imageUrl = null) : base(guildId, memberId)
            {
                BankId = bankId;
                Name = name;
                Value = value;
                Quantity = quantity;
                Description = description;
                ImageUrl = imageUrl;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildBankItem>
        {
            private readonly AldertoDbContext _context;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(AldertoDbContext context, IMediator mediator, IMapper mapper,
                ILogger<CommandHandler> logger)
            {
                _context = context;
                _mediator = mediator;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<GuildBankItem> Handle(Command request, CancellationToken cancellationToken)
            {
                var bank = await _context.GuildBanks.SingleOrDefaultAsync(b =>
                    b.GuildId == request.GuildId && b.Id == request.BankId, cancellationToken: cancellationToken);

                if (bank == null)
                    throw new ValidationDomainException(ErrorMessage.BANK_NOT_FOUND);

                var item = new GuildBankItem(request.Name)
                {
                    Description = request.Description,
                    Quantity = request.Quantity,
                    Value = request.Value,
                    GuildBankId = request.BankId,
                };

                _context.GuildBankItems.Add(item);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.Log(LogLevel.Information, 420,
                    "{GuildId}__{User} has created an item '{ItemName}' in the '{BankName}' bank", request.GuildId,
                    MentionUtils.MentionUser(request.MemberId), item.Name, bank.Name);

                return item;
            }
        }
    }
}
