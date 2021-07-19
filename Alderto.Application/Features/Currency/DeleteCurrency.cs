using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Application.Features.Currency
{
    public static class DeleteCurrency
    {
        public class Command : CommandRequest<Data.Models.Currency>
        {
            public Guid? Id { get; set; }

            [MaxLength(50)]
            public string? Name { get; }

            public Command(ulong guildId, ulong memberId, Guid id)
                : base(guildId, memberId)
            {
                Id = id;
            }


            public Command(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Data.Models.Currency>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Data.Models.Currency> Handle(Command request, CancellationToken cancellationToken)
            {
                var query = _context.Currencies.AsQueryable().Where(c => c.GuildId == request.GuildId);
                query = request.Id != null
                    ? query.Where(c => c.Id == request.Id)
                    : query.Where(c => c.Name == request.Name);

                var currency = await query.SingleOrDefaultAsync(cancellationToken);

                if (currency == null)
                    throw new ValidationDomainException($"Currency with the name '{request.Name}' was not found");

                _context.Currencies.Remove(currency);
                await _context.SaveChangesAsync(cancellationToken);

                return currency;
            }
        }
    }
}
