using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using AzureSearch.Entities;
using Persistence;

namespace AzureSearch.Services;
public class AzureSearchService
{
    private readonly ApplicationDbContext _context;
    private const string ServiceName = "bankmanager-search";
    private const string IndexName = "customers";
    private const string ApiKey = "C1F5A76E23B1F3B9437682452D8FF0A4";
    public SearchIndexClient AdminClient { get; set; }
    public SearchClient SearchClient{ get; set; }


    public AzureSearchService(ApplicationDbContext context)
    {
        _context = context;

        // Create a SearchIndexClient to send create/delete index commands
        var serviceEndpoint = new Uri($"https://{ServiceName}.search.windows.net/");
        var credential = new AzureKeyCredential(ApiKey);
        AdminClient = new SearchIndexClient(serviceEndpoint, credential);
        // Create a SearchClient to load and query documents
        SearchClient = new SearchClient(serviceEndpoint, IndexName, credential);
    }


    public void CreateIndex()
    {
        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(Customer));

        var definition = new SearchIndex(IndexName, searchFields);

        var suggester = new SearchSuggester("sg", new[] { "Givenname", "City/Country", "Address/City", "Address" });
        definition.Suggesters.Add(suggester);

        AdminClient.CreateOrUpdateIndex(definition);
    }





    //public void UploadDocuments()
    //{

    //    var customers = _context.Customers.ToList();

    //    var actions = new List<IndexDocumentsAction<Customer>>();

    //    customers.ForEach(x => actions.Add(new IndexDocumentsAction<Customer>(IndexActionType.Upload, new Customer() { City = x.City, Country = x.Country, Streetaddress = x.Streetaddress, Givenname = x.Givenname, Surname = x.Surname, Id = x.Id })));

    //    var batch = IndexDocumentsBatch.Create(actions.ToArray());


    //    try
    //    {
    //        IndexDocumentsResult result = SearchClient.IndexDocuments(batch);
    //    }
    //    catch (Exception)
    //    {
    //        // If for some reason any documents are dropped during indexing, you can compensate by delaying and
    //        // retrying. This simple demo just logs the failed document keys and continues.
    //        Console.WriteLine("Failed to index some of the documents: {0}");
    //    }
    //}


}
