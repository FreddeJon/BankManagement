namespace Application.Features.AzureSearch.Query.GetCustomersUsingAzureSearchService;

public class GetCustomersResponse : BaseResponse
{
    public IReadOnlyList<SearchCustomerDto> Customers { get; set; } = null!;
    public int Page { get; set; }
    public int TotalPage { get; set; }
}