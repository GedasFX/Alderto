using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Alderto.Application
{
    public abstract class Request<TOut> : IRequest<TOut>
    {
        /// <summary>
        /// Guild Id. If set to 0, means the request was outside of guild scope.
        /// </summary>
        [Required]
        public ulong GuildId { get; set; }

        /// <summary>
        /// Member id. If set to 0, means the request is anonymous.
        /// </summary>
        [Required]
        public ulong MemberId { get; set; }

        protected Request(ulong guildId, ulong memberId)
        {
            GuildId = guildId;
            MemberId = memberId;
        }
    }
}
