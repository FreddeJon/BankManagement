namespace Application.Features.Account.Query.GetAccountDetailsForTransaction;

public class GetAccountDetailsForTransactionResponse : BaseResponse
{
    public AccountDto Account { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public int CustomerId { get; set; }
}