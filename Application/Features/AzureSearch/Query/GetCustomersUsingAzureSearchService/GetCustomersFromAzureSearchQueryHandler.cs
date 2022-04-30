using Azure.Search.Documents;
using AzureSearch.Entities;
using AzureSearch.Services;

namespace Application.Features.AzureSearch.Query.GetCustomersUsingAzureSearchService;
public class GetCustomersFromAzureSearchQueryHandler : IRequestHandler<GetCustomersFromAzureSearchQuery, GetCustomersResponse>
{
    private readonly IAzureSearchService _azureSearchService;
    private readonly IMapper _mapper;

    public GetCustomersFromAzureSearchQueryHandler(IAzureSearchService azureSearchService, IMapper mapper)
    {
        _azureSearchService = azureSearchService;
        _mapper = mapper;
    }
    public async Task<GetCustomersResponse> Handle(GetCustomersFromAzureSearchQuery request, CancellationToken cancellationToken)
    {
        var response = new GetCustomersResponse();
        var options = new SearchOptions
        {
            IncludeTotalCount = true,
            Size = 50,
            Skip = (request.Page - 1) * 50
        };

        // If sortColumn is empty add "" else add column and order
        options.OrderBy.Add((GetCol(request.SortColumn) == "") ? "" : $"{GetCol(request.SortColumn)} {GetOrder(request.SortingOrder.ToString())}");

        var searchResponse = await _azureSearchService.SearchClient.SearchAsync<SearchCustomer>(request.Search, options, cancellationToken);

        if (searchResponse is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = "Search failed";
            return response;
        }

        response.Page = request.Page;
#pragma warning disable CS8629 // Nullable value type may be null.
        response.TotalPage = (int)Math.Ceiling(((double)searchResponse.Value.TotalCount)! / 50);
#pragma warning restore CS8629 // Nullable value type may be null.
        var hmm = searchResponse.Value.GetResults().ToList();
        response.Customers = _mapper.Map<IReadOnlyList<SearchCustomerDto>>(hmm);
        return response;
    }




    private static string GetOrder(string? order)
    {
        if (order is null) return "asc";

        return order.ToLower() switch
        {
            "desc" => "desc",
            _ => "asc"
        };
    }
    private static string GetCol(string currentCol)
    {
        if (currentCol is null) return "";

        return currentCol.ToLower() switch
        {
            "firstname" => "Givenname",
            "lastname" => "Surname",
            "address" => "Streetaddress",
            "city" => "City",
            "country" => "Country",
            _ => ""
        };
    }
}