using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Data;
using Alderto.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace Alderto.Application.Features.GuildCommandAlias
{
    public static class RemoveCommandAlias
    {
        public class RhCommand : Request<Model>
        {
            [Required, MaxLength(50)]
            public string Alias { get; }

            public RhCommand(ulong guildId, ulong memberId, string alias) : base(guildId, memberId)
            {
                Alias = alias;
            }
        }

        public class Model
        {
            public string Alias { get; set; }
            public string Command { get; set; }

            public Model(string alias, string command)
            {
                Alias = alias;
                Command = command;
            }
        }

        public class CommandHandler : IRequestHandler<RhCommand, Model>
        {
            private readonly AldertoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(AldertoDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Model> Handle(RhCommand request, CancellationToken cancellationToken)
            {
                var commandAlias = await _context.GuildCommandAliases.FindAsync(
                    new object[] { request.GuildId, request.Alias }, cancellationToken);

                if (commandAlias == null)
                    throw new BadRequestDomainException("Requested Alias was not found.");

                _context.GuildCommandAliases.Remove(commandAlias);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map<Model>(commandAlias);
            }
        }

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Data.Models.GuildCommandAlias, Model>();
            }
        }
    }
}