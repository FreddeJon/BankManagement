namespace Application.Models.DataTransferObjects;
public class AccountDto
{
    public int Id { get; set; }
    public string AccountType { get; set; } = null!;
    public DateTime Created { get; set; }
    public decimal Balance { get; set; }
    public DateTime? LatestTransaction { get; set; }
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<TransactionDto> Transactions { get; set; } = new();
}