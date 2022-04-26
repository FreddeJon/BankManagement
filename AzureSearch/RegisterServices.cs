using AzureSearch.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AzureSearch;
public static class RegisterServices
{
    public static void ConfigureAzureSearch(this IServiceCollection services)
    {
        services.AddTransient<AzureSearchService>();
    }
}
