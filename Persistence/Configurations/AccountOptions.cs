// ReSharper disable ClassNeverInstantiated.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Persistence.Configurations;
public class AccountOptions
{
    public bool SeedAccounts { get; set; }
    public AdminOptions? AdminOptions { get; set; }
    public UserOptions? UserOptions { get; set; }
}

public abstract class BaseAccountOptions
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class AdminOptions : BaseAccountOptions
{
}

public class UserOptions : BaseAccountOptions
{
}