namespace Application.Features.Statistics.Query;

public class Statistic
{
    public string Country { get; init; } = null!;
    public string CountryCode { get; init; } = null!;
    public int TotalAccounts { get; init; }
    public int TotalCustomers { get; init; }
    public decimal TotalBalance { get; init; }
    public double Percentage { get; init; }
}