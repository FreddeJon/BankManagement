namespace Application.Models.DataTransferObjects;

public class TransactionDto
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public string Operation { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public decimal NewBalance { get; set; }
}