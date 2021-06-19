using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using AutoMapper;
using MediatR;

namespace Alderto.Application.Features.GuildCommandAlias
{
    public static class RegisterCommandAlias
    {
        public class RhCommand : Request
        {
            [MaxLength(50)]
            public string Alias { get; }

            [Required, MaxLength(2000)]
            public string Command { get; }

            public RhCommand(ulong guildId, ulong memberId, string alias, string command) : base(guildId, memberId)
            {
                Alias = alias;
                Command = command;
            }
        }

        public class CommandHandler : IRequestHandler<RhCommand>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(RhCommand request, CancellationToken cancellationToken)
            {
                var commandAlias = _mapper.Map<Data.Models.GuildCommandAlias>(request);

                _context.GuildCommandAliases.Add(commandAlias);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<RhCommand, Data.Models.GuildCommandAlias>();
            }
        }
    }
}