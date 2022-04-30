
using AzureSearch;
using Microsoft.OpenApi.Models;

namespace WebApp;
public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.Services.ConfigurePersistenceServices(builder.Configuration);
        builder.Services.ConfigureApplicationServices();
        builder.Services.ConfigureAzureSearch();

        builder.Services.AddResponseCaching();

        builder.Services.AddControllers();



        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                policy => policy.RequireRole(nameof(ApplicationRoles.Admin)));
        });

        builder.Services.AddRazorPages().AddRazorPagesOptions(ops =>
        {
            ops.Conventions.AuthorizeFolder("/");
            ops.Conventions.AuthorizeFolder("/Users", "Admin");
            ops.Conventions.AllowAnonymousToAreaPage("Identity", "/Accounts/Login");
        });

        builder.Services.AddServerSideBlazor();

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}