using Application.Infrastructure.Paging;

namespace Application.Features.Customer.Query.GetCustomersPaginated;
public class GetCustomerPaginatedQuery : IRequest<GetCustomersPaginatedBaseResponse>
{
    public string? Search { get; set; }
    public int Page { get; init; }
    public int Limit { get; init; }
    public string SortColumn { get; init; } = "Id";
    public SortingOrder SortingOrder { get; init; } = SortingOrder.Asc;
}
