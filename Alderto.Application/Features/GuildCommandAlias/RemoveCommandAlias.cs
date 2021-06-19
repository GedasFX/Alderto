using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Domain.Services;
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
            private readonly IGuildSetupService _setupService;
            private readonly IMapper _mapper;

            public CommandHandler(IGuildSetupService setupService, IMapper mapper)
            {
                _setupService = setupService;
                _mapper = mapper;
            }

            public async Task<Model> Handle(RhCommand request, CancellationToken cancellationToken)
            {
                var commandAlias = await _setupService.RemoveCommandAlias(request.GuildId, request.Alias);
                return _mapper.Map<Model>(commandAlias);
            }
        }
    }
}