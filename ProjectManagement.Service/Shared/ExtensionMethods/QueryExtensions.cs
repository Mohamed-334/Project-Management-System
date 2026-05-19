using System.Linq.Expressions;

namespace ProjectManagement.Service.Shared.ExtensionMethods
{
    public static class QueryExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
            => condition ? query.Where(predicate) : query;
    }
}
