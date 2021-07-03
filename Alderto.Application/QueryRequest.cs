using System.ComponentModel.DataAnnotations;

namespace Alderto.Application
{
    public abstract class QueryRequest<TOut> : Request<TOut>
    {
        protected QueryRequest(ulong guildId, ulong memberId) : base(guildId, memberId)
        {
        }
    }

    public abstract class PagedQueryRequest<TOut> : QueryRequest<TOut>
    {
        [Range(1, int.MaxValue)]
        public virtual int Page { get; }

        [Range(1, int.MaxValue)]
        public virtual int Take { get; }

        protected PagedQueryRequest(ulong guildId, ulong memberId, int page = 1, int take = 100)
            : base(guildId, memberId)
        {
            Page = page;
            Take = take;
        }
    }
}
