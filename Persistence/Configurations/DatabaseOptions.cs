﻿namespace Persistence.Configurations;

public class DatabaseOptions
{
    public bool SeedCustomerUserAccount { get; set; }
    public bool SeedDatabase { get; set; }
    public int CustomersToSeed { get; set; }
}