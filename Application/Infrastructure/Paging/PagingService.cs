using System.Linq.Expressions;

namespace Application.Infrastructure.Paging;

public static class PagingService
{
    public static async Task<PagingResponse<T>> HandlePaging<T>(this IQueryable<T> query, int page, int limit) where T : class
    {
        var response = new PagingResponse<T>
        {
            Limit = limit,
            Page = page,
            TotalPage = (int)Math.Ceiling((double)query.Count() / limit),
            Results = await query.Skip((page - 1) * limit).Take(limit).ToListAsync()
        };
        return response;
    }

    public static IQueryable<T> HandleSorting<T>(this IQueryable<T> query, SortingOrder sortingOrder, string sortColumn) where T : class
    {
        return sortingOrder == SortingOrder.Desc
            ? query.OrderByDescending(ToLambda<T>(sortColumn))
            : query.OrderBy(ToLambda<T>(sortColumn));
    }


    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}