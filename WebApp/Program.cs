using AzureSearch;
using Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureServices();
var app = builder.Build();



// Init Search and data
await app.Services.InitializeDataAsync();
await app.Services.InitializeAzureSearch();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCaching();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
    endpoint.MapRazorPages();
});


app.Run();