namespace Persistence.Options;

public class DatabaseOptions
{
    public bool SeedCustomerUserAccount { get; set; }
    public bool SeedDatabase { get; set; }
    public int CustomersToSeed { get; set; }
}