namespace AzureSearch.Services;
public interface IAzureSearchService
{
    public SearchClient SearchClient { get; }
    Task CreateIndex();
    Task<bool> UploadDocuments();
}
