using System.ComponentModel.DataAnnotations;

namespace Alderto.Application
{
    public abstract class PagedQuery : Request
    {
        [Range(1, int.MaxValue)]
        protected virtual int Page { get; }

        [Range(1, int.MaxValue)]
        protected virtual int Take { get; }

        protected PagedQuery(ulong guildId, ulong memberId, int page = 1, int take = 25) : base(guildId, memberId)
        {
            Page = page;
            Take = take;
        }
    }

    public abstract class PagedQuery<TOut> : Request<TOut>
    {
        [Range(1, int.MaxValue)]
        public virtual int Page { get; }

        [Range(1, int.MaxValue)]
        public virtual int Take { get; }

        protected PagedQuery(ulong guildId, ulong memberId, int page = 1, int take = 25) : base(guildId, memberId)
        {
            Page = page;
            Take = take;
        }
    }
}
