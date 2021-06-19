using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Alderto.Application
{
    public abstract class Request : RequestBase, IRequest
    {
        protected Request(ulong guildId, ulong memberId)
        {
            GuildId = guildId;
            MemberId = memberId;
        }
    }

    public abstract class Request<TOut> : RequestBase, IRequest<TOut>
    {
        protected Request(ulong guildId, ulong memberId)
        {
            GuildId = guildId;
            MemberId = memberId;
        }
    }

    public abstract class RequestBase
    {
        [Required]
        protected internal ulong GuildId { get; protected init; }

        [Required]
        protected internal ulong MemberId { get; protected init; }
    }
}