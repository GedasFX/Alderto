using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Alderto.Data
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query,
            [Range(1, int.MaxValue)] int page,
            [Range(1, int.MaxValue)] int itemsPerPage) =>
            query.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
    }
}
