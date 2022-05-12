namespace Application.Features.Api.Query.GetAccountForCustomer;

public class GetAccountForCustomerResponse : BaseResponse
{
    public int TotalTransactions { get; set; }
    public int TotalPages { get; set; }
    public AccountDto Account { get; set; } = null!;
}