namespace Application.Features.Customer.Query.GetCustomerByIdIncludeAccounts;
public class GetCustomerQuery : IRequest<GetCustomerBaseResponse>
{
    public int CustomerId { get; init; }
}