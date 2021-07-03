using MediatR;

namespace Alderto.Application
{
    public abstract class CommandRequest<TOut> : Request<TOut>
    {
        protected CommandRequest(ulong guildId, ulong memberId) : base(guildId, memberId)
        {
        }
    }

    public abstract class CommandRequest : CommandRequest<Unit>
    {
        protected CommandRequest(ulong guildId, ulong memberId) : base(guildId, memberId)
        {
        }
    }
}
