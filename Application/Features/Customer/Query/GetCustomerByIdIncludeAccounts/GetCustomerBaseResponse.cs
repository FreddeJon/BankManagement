namespace Application.Features.Customer.Query.GetCustomerByIdIncludeAccounts;

public class GetCustomerBaseResponse : BaseResponse
{
    public CustomerDto? Customer { get; init; }
}