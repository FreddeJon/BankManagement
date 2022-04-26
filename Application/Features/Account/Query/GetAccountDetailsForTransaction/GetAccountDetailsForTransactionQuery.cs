namespace Application.Features.Account.Query.GetAccountDetailsForTransaction;
public class GetAccountDetailsForTransactionQuery : IRequest<GetAccountDetailsForTransactionResponse>
{
    public int AccountId { get; init; }
    public int CustomerId { get; init; }
}