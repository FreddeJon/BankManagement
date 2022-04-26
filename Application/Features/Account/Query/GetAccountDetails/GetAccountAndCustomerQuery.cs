namespace Application.Features.Account.Query.GetAccountDetails;
public class GetAccountAndCustomerQuery : IRequest<GetAccountResponse>
{
    public int CustomerId { get; init; }
    public int AccountId { get; init; }
}