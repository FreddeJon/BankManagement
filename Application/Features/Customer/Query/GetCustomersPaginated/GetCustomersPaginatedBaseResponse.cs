// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Features.Customer.Query.GetCustomersPaginated;

public class GetCustomersPaginatedBaseResponse : BaseResponse
{
    public IReadOnlyList<CustomerDto> Customers { get; set; } = null!;
    public int Page { get; set; }
    public int TotalPage { get; set; }
    public int Limit { get; set; }
}