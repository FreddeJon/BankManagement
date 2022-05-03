using MoneyLaunderingBatch.Configurations;
using MoneyLaunderingBatch.Contracts;
using MoneyLaunderingBatch.Services;

namespace MoneyLaunderingBatch;

public static class RegisterServices
{
    public static IConfiguration ConfigureMoneyLaunderingBatch(this IServiceCollection services)
    {
        var path = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName + @"\WebApp";

        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>();
        IConfiguration configuration = builder.Build();
        services.Configure<MoneyLaunderingEmailOptions>(
            configuration.GetSection("MoneyLaunderingEmailOptions"));


        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<ILaunderingChecker, LaunderingChecker>();
        return configuration;
    }
}

