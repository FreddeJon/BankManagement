using System.Text;
using AzureSearch;
using Microsoft.IdentityModel.Tokens;
using Persistence.Contracts;
using Shared;

namespace WebApp;
public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var sharedConfigurations = RegisterSharedSettings.GetSharedSettings();


        builder.Services.ConfigurePersistenceServices();
        builder.Services.ConfigureApplicationServices();
        builder.Services.ConfigureAzureSearch();

        builder.Services.AddResponseCaching();

        builder.Services.AddControllers();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                policy => policy.RequireRole(nameof(ApplicationRoles.Admin)));

            options.AddPolicy("AdminCashier",
                policy => policy.RequireRole(nameof(ApplicationRoles.Cashier)));
        });

        builder.Services.AddRazorPages().AddRazorPagesOptions(ops =>
        {
            ops.Conventions.AuthorizeFolder("/", "AdminCashier");
            ops.Conventions.AuthorizeFolder("/Users", "Admin");
        });
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddAuthentication()
            .AddCookie(x => x.SlidingExpiration = true)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = sharedConfigurations["JwtOptions:ValidAudience"],
                    ValidIssuer = sharedConfigurations["JwtOptions:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sharedConfigurations["JwtOptions:Secret"]))
                };
            });
    }
}