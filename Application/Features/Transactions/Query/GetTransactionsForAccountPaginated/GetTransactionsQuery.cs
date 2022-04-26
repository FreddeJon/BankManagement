namespace Application.Features.Transactions.Query.GetTransactionsForAccountPaginated;
public class GetTransactionsQuery : IRequest<GetTransactionsResponse>
{
    public int AccountId { get; init; }
    public int Page { get; init; }
}