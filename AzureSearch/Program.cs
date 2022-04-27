//using Azure;
//using Azure.Search.Documents;
//using Azure.Search.Documents.Indexes.Models;
//using Azure.Search.Documents.Models;
//using AzureSearch.Entities;
//using Microsoft.Azure.Search;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Persistence;
//using FieldBuilder = Azure.Search.Documents.Indexes.FieldBuilder;
//using SearchIndexClient = Azure.Search.Documents.Indexes.SearchIndexClient;


//namespace AzureSearch;

//class Program
//{


//    static async Task Main(string[] args)
//    {
//        // Create a SearchIndexClient to send create/delete index commands
//        var serviceEndpoint = new Uri($"https://{ServiceName}.search.windows.net/");
//        var credential = new AzureKeyCredential(ApiKey);
//        var adminClient = new SearchIndexClient(serviceEndpoint, credential);

//        // Create a SearchClient to load and query documents
//        var searchClient = new SearchClient(serviceEndpoint, IndexName, credential);
//        await CreateIndex(adminClient);
//    }


//    private static async Task CreateIndex(SearchIndexClient adminClient)
//    {
//        await adminClient.DeleteIndexAsync(IndexName);
//        var fieldBuilder = new FieldBuilder();
//        var searchFields = fieldBuilder.Build(typeof(Customer));

//        var definition = new SearchIndex(IndexName, searchFields);

//        await adminClient.CreateOrUpdateIndexAsync(definition);
//    }


//    public void UploadDocuments(SearchClient searchClient)
//    {
//        var customers = _context.Customers.ToList();

//        var actions = new List<IndexDocumentsAction<Customer>>();

//        customers.ForEach(x => actions.Add(new IndexDocumentsAction<Customer>(IndexActionType.Upload, new Customer() { City = x.City, Country = x.Country, Streetaddress = x.Streetaddress, Givenname = x.Givenname, Surname = x.Surname, Id = x.Id.ToString() })));

//        var batch = IndexDocumentsBatch.Create(actions.ToArray());


//        try
//        {
//            IndexDocumentsResult result = searchClient.IndexDocuments(batch);
//        }
//        catch (Exception)
//        {
//            // If for some reason any documents are dropped during indexing, you can compensate by delaying and
//            // retrying. This simple demo just logs the failed document keys and continues.
//            Console.WriteLine("Failed to index some of the documents: {0}");
//        }
//    }

//}


using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using AzureSearch.Entities;
using Microsoft.Extensions.Configuration;

namespace AzureSearch.Quickstart

{
    class Program
    {


        static void Main(string[] args)
        {

            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();

            SearchIndexClient indexClient = CreateSearchIndexClient(configuration);

            string indexName = configuration["SearchIndexName"];

            Console.WriteLine("{0}", "Deleting index...\n");
            DeleteIndexIfExists(indexName, indexClient);

            Console.WriteLine("{0}", "Creating index...\n");
            CreateIndex(indexName, indexClient);

            SearchClient searchClient = indexClient.GetSearchClient(indexName);

            Console.WriteLine("{0}", "Uploading documents...\n");
            UploadDocuments(searchClient);

            //SearchClient indexClientForQueries = CreateSearchClientForQueries(indexName, configuration);

            Console.WriteLine("{0}", "Run queries...\n");
            //RunQueries(indexClientForQueries);

            Console.WriteLine("{0}", "Complete.  Press any key to end application...\n");
            Console.ReadKey();
        }
        private static SearchIndexClient CreateSearchIndexClient(IConfigurationRoot configuration)
        {
            string searchServiceEndPoint = configuration["SearchServiceEndPoint"];
            string adminApiKey = configuration["SearchServiceAdminApiKey"];

            SearchIndexClient indexClient = new SearchIndexClient(new Uri(searchServiceEndPoint), new AzureKeyCredential(adminApiKey));
            return indexClient;
        }

        // Delete the hotels-quickstart index to reuse its name
        private static void DeleteIndexIfExists(string indexName, SearchIndexClient adminClient)
        {
            adminClient.GetIndexNames();
            {
                adminClient.DeleteIndex(indexName);
            }
        }
        // Create hotels-quickstart index
        private static void CreateIndex(string indexName, SearchIndexClient adminClient)
        {
            FieldBuilder fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(Customer));

            var definition = new SearchIndex(indexName, searchFields);

            var suggester = new SearchSuggester("sg", new[] { "Givenname", "Surname", "City", "Country" });
            definition.Suggesters.Add(suggester);

            adminClient.CreateOrUpdateIndex(definition);
        }

        // Upload documents in a single Upload request.
        private static void UploadDocuments(SearchClient searchClient)
        {
            IndexDocumentsBatch<Customer> batch = IndexDocumentsBatch.Create(
                IndexDocumentsAction.Upload(new Customer() { City = "Stockholm", Country = "Sweden", Givenname = "Fredrik", Surname = "Jonson", Id = "1", Streetaddress = "Tennisvägen" })
            );

            try
            {
                IndexDocumentsResult result = searchClient.IndexDocuments(batch);
            }
            catch (Exception)
            {
                // If for some reason any documents are dropped during indexing, you can compensate by delaying and
                // retrying. This simple demo just logs the failed document keys and continues.
                Console.WriteLine("Failed to index some of the documents: {0}");
            }
        }

        // Run queries, use WriteDocuments to print output
        private static void RunQueries(SearchClient srchclient)
        {
            SearchOptions options;
            SearchResults<Customer> response;

            // Query 1
            Console.WriteLine("Query #1: Search on empty term '*' to return all documents, showing a subset of fields...\n");

            options = new SearchOptions()
            {
                IncludeTotalCount = true,
                Filter = "",
                OrderBy = { "" }
            };

            options.Select.Add("Id");
            options.Select.Add("Givenname");

            response = srchclient.Search<Customer>("*", options);
            WriteDocuments(response);


        }

        // Write search results to console
        private static void WriteDocuments(SearchResults<Customer> searchResults)
        {
            foreach (SearchResult<Customer> result in searchResults.GetResults())
            {
                Console.WriteLine(result.Document.Givenname);
            }

            Console.WriteLine();
        }

        private static void WriteDocuments(AutocompleteResults autoResults)
        {
            foreach (AutocompleteItem result in autoResults.Results)
            {
                Console.WriteLine(result.Text);
            }

            Console.WriteLine();
        }
    }
}