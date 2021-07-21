using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Alderto.Application
{
    public abstract class QueryRequest<TOut> : Request<TOut>
    {
        protected QueryRequest(ulong guildId, ulong memberId) : base(guildId, memberId)
        {
        }
    }

    public abstract class PagedQueryRequest<TOut> : QueryRequest<PagedResponse<TOut>>
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

    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public int Count { get; set; }
        public int Limit { get; set; }

        public ICollection<T> Items { get; set; }

        public PagedResponse(int page, int limit, int count, ICollection<T> items)
        {
            Page = page;
            Count = count;
            Limit = limit;
            Items = items;
        }
    }
}
