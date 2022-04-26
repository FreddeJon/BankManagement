﻿namespace Persistence;
public static class RegisterPersistenceServices
{
    public static void ConfigurePersistenceServices(this IServiceCollection service, IConfiguration configuration, bool isDevelopment)
    {
        var connection = configuration.GetConnectionString(isDevelopment ? "DevConnection" : "DbConnection");


        service.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connection));
        service.AddDatabaseDeveloperPageExceptionFilter();

        service.AddDefaultIdentity<IdentityUser>(options =>
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

        configuration.GetSection("");

        service.Configure<DatabaseOptions>(
            configuration.GetSection("DatabaseOptions"));

        service.Configure<AccountOptions>(
            configuration.GetSection("AccountOptions"));
    }
}
