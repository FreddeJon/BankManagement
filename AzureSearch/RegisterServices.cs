namespace AzureSearch;
public static class RegisterServices
{
    public static IConfiguration ConfigureAzureSearch(this IServiceCollection services)
    {
        var path = Directory.GetCurrentDirectory().Contains("AzureSearch")
            ? Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName + @"\WebApp"
            : Directory.GetCurrentDirectory();


        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>();
        IConfiguration configuration = builder.Build();

        services.Configure<AzureSearchOptions>(
            configuration.GetSection("AzureSearchOptions"));
        services.AddTransient<IAzureSearchService, AzureSearchService>();

        return configuration;
    }
}