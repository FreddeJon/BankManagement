namespace AzureSearch;
public static class RegisterServices
{
    public static IConfiguration ConfigureAzureSearch(this IServiceCollection services)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>();
        IConfiguration configuration = builder.Build();

        services.Configure<AzureSearchOptions>(
            configuration.GetSection("AzureSearchOptions"));
        services.AddTransient<IAzureSearchService, AzureSearchService>();

        return configuration;
    }
}