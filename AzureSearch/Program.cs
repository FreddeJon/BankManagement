using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        {
            var configuration = services.ConfigureAzureSearch();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DbConnection"), contextBuilder => contextBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            });
            services.AddScoped<IAzureSearchService, AzureSearchService>();
        }
    ).Build();

var scope = host.Services.CreateAsyncScope();

var azureSearchService = scope.ServiceProvider.GetService<IAzureSearchService>();

if (azureSearchService is not null)
{
    await azureSearchService.CreateIndex();
    await azureSearchService.UploadDocuments();
}
else
{
    Console.WriteLine("Error in AzureSearchService");
}


Console.Read();