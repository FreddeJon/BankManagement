// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Infrastructure.Paging;

public class PagingResponse<T> where T : class
{
    public int Page { get; init; }
    public int TotalPage { get; init; }
    public int Limit { get; init; }
    public IReadOnlyList<T> Results { get; init; } = null!;
}