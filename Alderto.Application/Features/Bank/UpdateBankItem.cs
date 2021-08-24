using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Application.Features.Bank.Events;
using Alderto.Data;
using Alderto.Data.Models;
using Alderto.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Bank
{
    public static class UpdateBankItem
    {
        public class Command : CommandRequest<GuildBankItem>
        {
            [Range(1, int.MaxValue, ErrorMessage = "Invalid item id")]
            public int Id { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Invalid bank id")]
            public int BankId { get; set; }

            [MaxLength(70)]
            public string? Name { get; set; }

            public double? Value { get; set; }

            public double? Quantity { get; set; }

            [MaxLength(280)]
            public string? Description { get; set; }

            [MaxLength(140)]
            public string? ImageUrl { get; set; }

            public Command(ulong guildId, ulong memberId, int id, int bankId, string? name = null, double? value = null,
                double? quantity = null, string? description = null, string? imageUrl = null) : base(guildId, memberId)
            {
                Id = id;
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

            public CommandHandler(AldertoDbContext context, IMediator mediator, IMapper mapper)
            {
                _context = context;
                _mediator = mediator;
                _mapper = mapper;
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

                _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new BankItemUpdatedEvent(item, request), cancellationToken);

                return item;
            }
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, GuildBankItem>()
                    .ForAllMembers(o => o.Condition((_, _, m) => m != null));
            }
        }
    }
}
