using Application.Infrastructure.Paging;

namespace Application.Features.AzureSearch.Query.GetCustomersUsingAzureSearchService;
public class GetCustomersFromAzureSearchQuery : IRequest<GetCustomersResponse>
{
    public string? Search { get; set; }
    public int Page { get; init; }
    public string SortColumn { get; init; } = "Id";
    public SortingOrder SortingOrder { get; init; } = SortingOrder.Asc;
}