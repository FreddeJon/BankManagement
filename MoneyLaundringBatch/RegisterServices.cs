namespace MoneyLaunderingBatch;

public static class RegisterServices
{
    public static IConfiguration ConfigureMoneyLaunderingBatch(this IServiceCollection services)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>();
        IConfiguration configuration = builder.Build();
        return configuration;
    }
}

