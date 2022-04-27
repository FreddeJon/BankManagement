
namespace WebApp;
public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.Services.ConfigurePersistenceServices(builder.Configuration, builder.Environment.IsDevelopment());
        builder.Services.ConfigureApplicationServices();
       
        
        
        builder.Services.AddResponseCaching();
        builder.Services.AddRazorPages().AddRazorPagesOptions(ops =>
        {
            ops.Conventions.AuthorizeFolder("/");
            ops.Conventions.AllowAnonymousToAreaPage("Identity", "/Accounts/Login");
        });

        //builder.Services.AddServerSideBlazor();

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}