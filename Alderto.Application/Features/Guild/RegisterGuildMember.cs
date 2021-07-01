using System.Threading;
using System.Threading.Tasks;
using Alderto.Domain.Services;
using MediatR;

namespace Alderto.Application.Features.Guild
{
    public static class RegisterGuildMember
    {
        public class Command : Request
        {
            public Command(ulong guildId, ulong memberId) : base(guildId, memberId)
            {
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly IGuildMemberService _userService;

            public CommandHandler(IGuildMemberService userService)
            {
                _userService = userService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // This method efficiently adds all missing links in the database.
                await _userService.GetGuildMemberAsync(request.GuildId, request.MemberId);
                return Unit.Value;
            }
        }
    }
}
