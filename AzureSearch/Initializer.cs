namespace AzureSearch;
public static class Initializer
{
    public static async Task InitializeAzureSearch(this IServiceProvider service)
    {
        using var scope = service.CreateScope();

        var azureSearchService = scope.ServiceProvider.GetService<IAzureSearchService>();

        if (azureSearchService is not null)
        {
            await azureSearchService.CreateIndex();
            await azureSearchService.UploadDocuments();
        }
        else
        {
            throw new ArgumentNullException(nameof(azureSearchService));
        }
    }
}
