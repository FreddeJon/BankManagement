namespace Application.Features.AzureSearch.Query.GetCustomersUsingAzureSearchService;

public class SearchCustomerDto
{
    public int Id { get; set; }
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string NationalId { get; set; } = null!;
}