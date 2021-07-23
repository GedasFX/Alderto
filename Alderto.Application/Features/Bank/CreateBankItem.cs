using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank.Events;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            [MaxLength(140), Url]
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

            public CommandHandler(AldertoDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
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
                    ImageUrl = request.ImageUrl,
                };

                _context.GuildBankItems.Add(item);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new BankItemCreatedEvent(item, request), cancellationToken);

                return item;
            }
        }
    }
}
