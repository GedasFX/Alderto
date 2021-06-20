using System;
using System.ComponentModel.DataAnnotations;
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
        public class Command : Request<Model>
        {
            [MaxLength(50)]
            public string Name { get; }

            public Command(ulong guildId, ulong memberId, string name) : base(guildId, memberId)
            {
                Name = name;
            }
        }

        public class Model
        {
            public Guid Id { get; }
            public ulong GuildId { get; }
            public string Symbol { get; }
            public string Name { get; }
            public string? Description { get; }
            public int? TimelyInterval { get; }

            public Model(Guid id, ulong guildId, string symbol, string name, string? description,
                int? timelyInterval)
            {
                Id = id;
                GuildId = guildId;
                Symbol = symbol;
                Name = name;
                Description = description;
                TimelyInterval = timelyInterval;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Model>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                var currency =
                    await _context.Currencies.SingleOrDefaultAsync(c =>
                        c.GuildId == request.GuildId && c.Name == request.Name, cancellationToken: cancellationToken);

                if (currency == null)
                    throw new BadRequestDomainException($"Currency with the name '{request.Name}' was not found");

                _context.Currencies.Remove(currency);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<Model>(currency);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.Currency, Model>();
            }
        }
    }
}