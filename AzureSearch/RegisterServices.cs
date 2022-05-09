using AzureSearch.Options;
using Shared;

namespace AzureSearch;
public static class RegisterServices
{
    public static IConfiguration ConfigureAzureSearch(this IServiceCollection services)
    {
        var sharedConfigurations = RegisterSharedSettings.GetSharedSettings();

        services.Configure<AzureSearchOptions>(sharedConfigurations.GetSection(nameof(AzureSearchOptions)));

        services.AddTransient<IAzureSearchService, AzureSearchService>();

        return sharedConfigurations;
    }
}