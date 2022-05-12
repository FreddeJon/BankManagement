namespace Application.Features.Api.Query.GetAccountForCustomer;
public class GetAccountForCustomerQuery : IRequest<GetAccountForCustomerResponse>
{
    public string UserId { get; init; } = null!;
    public int AccountId { get; init; }
    public int Limit { get; init; }
    public int Offset { get; init; }
}