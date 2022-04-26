namespace Application.Features.Transactions.Commands.Transfer;
public class TransferCommand : IRequest<TransferResponse>
{
    public int ToAccountId { get; init; }
    public int FromAccountId { get; init; }
    public decimal Amount { get; init; }
}