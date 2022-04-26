namespace Application.Features.Transactions.Commands.Withdraw;
public class WithdrawCommand : IRequest<WithdrawResponse>
{
    public string? Operation { get; init; }
    public decimal Amount { get; init; }
    public int AccountId { get; init; }
}