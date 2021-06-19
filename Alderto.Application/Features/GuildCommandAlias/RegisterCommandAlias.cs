using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Alderto.Domain.Services;
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
            private readonly IGuildSetupService _setupService;

            public CommandHandler(IGuildSetupService setupService)
            {
                _setupService = setupService;
            }

            public async Task<Unit> Handle(RhCommand request, CancellationToken cancellationToken)
            {
                await _setupService.CreateCommandAlias(request.GuildId, request.Alias, request.Command);
                return Unit.Value;
            }
        }
    }
}