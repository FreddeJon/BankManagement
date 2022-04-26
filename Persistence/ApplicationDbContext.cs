// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618
namespace Persistence;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

}