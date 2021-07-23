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
    public static class UpdateBank
    {
        public class Command : CommandRequest<GuildBank>
        {
            [Range(1, int.MaxValue)]
            public int Id { get; set; }

            [MaxLength(32)]
            public string? Name { get; set; }

            public Command(ulong guildId, ulong memberId, int id, string? name = null) : base(guildId, memberId)
            {
                Name = name;
                Id = id;
            }
        }

        public class CommandHandler : IRequestHandler<Command, GuildBank>
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

            public async Task<GuildBank> Handle(Command request, CancellationToken cancellationToken)
            {
                var bank = await _context.GuildBanks.AsQueryable().Where(b => b.GuildId == request.GuildId)
                    .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken: cancellationToken);

                if (bank == null)
                    throw new ValidationDomainException(ErrorMessage.BANK_NOT_FOUND);

                _mapper.Map(request, bank);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new BankUpdatedEvent(bank, request), cancellationToken);

                return bank;
            }
        }

        private class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, GuildBank>()
                    .ForAllMembers(o => o.Condition((_, _, m) => m != null));
            }
        }
    }
}
