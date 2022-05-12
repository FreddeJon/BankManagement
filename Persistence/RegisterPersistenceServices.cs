using Persistence.Options;
using Shared;

namespace Persistence;
public static class RegisterPersistenceServices
{
    public static void ConfigurePersistenceServices(this IServiceCollection services)
    {

        var sharedConfiguration = RegisterSharedSettings.GetSharedSettings();

        var connection = sharedConfiguration.GetConnectionString("DbConnection");



        services.Configure<AccountOptions>(sharedConfiguration.GetSection(nameof(AccountOptions)));
        services.Configure<DatabaseOptions>(sharedConfiguration.GetSection(nameof(DatabaseOptions)));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connection), ServiceLifetime.Transient);
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = false;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

    }
}
