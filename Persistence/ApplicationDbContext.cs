// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

#pragma warning disable CS8618
namespace Persistence;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly IUserService _userService;
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<CustomerUser> CustomerUser { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserService userService)
        : base(options)
    {
        _userService = userService;
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {

        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
            case EntityState.Modified:
                entry.Entity.LastModifiedDate = DateTime.UtcNow;
                entry.Entity.LastModifiedBy = _userService.GetCurrentUser();
                break;
            case EntityState.Added:
                entry.Entity.CreatedBy = _userService.GetCurrentUser();
                entry.Entity.LastModifiedBy = _userService.GetCurrentUser();
                entry.Entity.LastModifiedDate = DateTime.UtcNow;
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}