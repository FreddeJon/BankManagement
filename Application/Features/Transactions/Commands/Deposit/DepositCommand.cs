namespace Application.Features.Transactions.Commands.Deposit;
public class DepositCommand : IRequest<DepositResponse>
{
    public int AccountId { get; init; }
    public decimal Amount { get; init; }
    public string Operation { get; init; } = null!;
}