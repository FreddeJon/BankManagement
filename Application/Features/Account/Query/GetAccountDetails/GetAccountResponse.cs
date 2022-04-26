namespace Application.Features.Account.Query.GetAccountDetails;

public class GetAccountResponse : BaseResponse
{
    public AccountDto? Account { get; set; }
    public CustomerDto? Customer { get; set; }
    public int TotalPages { get; set; }
    public int TotalTransactions { get; set; }
}