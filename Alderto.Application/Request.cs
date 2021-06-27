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
        /// <summary>
        /// Guild Id. If set to 0, means the request was outside of guild scope.
        /// </summary>
        [Required]
        public ulong GuildId { get; protected init; }

        /// <summary>
        /// Member id. If set to 0, means the request is anonymous.
        /// </summary>
        [Required]
        public ulong MemberId { get; protected init; }
    }
}
