
using AzureSearch.Options;

namespace AzureSearch.Services;
public class AzureSearchService : IAzureSearchService
{
    private readonly ApplicationDbContext _context;
    private readonly AzureSearchOptions _azureOptions;
    private readonly AzureKeyCredential _credential;
    private readonly Uri _serviceEndpoint;
    private readonly SearchIndexClient _adminClient;

    public SearchClient SearchClient { get; }

    public AzureSearchService(ApplicationDbContext context, IOptions<AzureSearchOptions> options)
    {
        _context = context;
        _azureOptions = options.Value;
        _credential = new AzureKeyCredential(_azureOptions.ApiKey);
        _serviceEndpoint = new Uri($"https://{_azureOptions.ServiceName}.search.windows.net/");
        _adminClient = CreateAdminClient();
        SearchClient = CreateSearchClient();
    }


    private SearchIndexClient CreateAdminClient()
    {
        return new SearchIndexClient(_serviceEndpoint, _credential);
    }

    private SearchClient CreateSearchClient()
    {
        return new SearchClient(_serviceEndpoint, _azureOptions.IndexName, _credential);
    }

    public async Task CreateIndex()
    {
        Console.WriteLine("Deleting old index");
        await _adminClient.DeleteIndexAsync(_azureOptions.IndexName);
        Console.WriteLine("Index deleted");

        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchCustomer));

        var definition = new SearchIndex(_azureOptions.IndexName, searchFields);

        var suggester = new SearchSuggester("sg", "Givenname", "Surname", "City", "Country");
        definition.Suggesters.Add(suggester);
        Console.WriteLine("Creating new index");
        await _adminClient.CreateOrUpdateIndexAsync(definition);
        Console.WriteLine("Index created");
    }


    public async Task<bool> UploadDocuments()
    {

        Console.WriteLine("Writing Batch");

        var customers = _context.Customers.ToList();

        var actions = new List<IndexDocumentsAction<SearchCustomer>>();

        customers.ForEach(x => actions.Add(new IndexDocumentsAction<SearchCustomer>(IndexActionType.Upload, new SearchCustomer() { City = x.City, Country = x.Country, Streetaddress = x.Streetaddress, Givenname = x.Givenname, Surname = x.Surname, Id = x.Id.ToString(), NationalId = x.NationalId })));

        var batch = IndexDocumentsBatch.Create(actions.ToArray());

        try
        {
            await SearchClient.IndexDocumentsAsync(batch);
            Console.WriteLine("Batch Completed");
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to index some of the documents: {0}");
            return false;
        }



        return true;
    }
}
