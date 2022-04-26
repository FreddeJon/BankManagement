namespace Application.Features.Transactions.Query.GetTransactionsForAccountPaginated;

public class GetTransactionsResponse : BaseResponse
{
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public IReadOnlyList<TransactionDto> Transactions { get; set; } = null!;
}