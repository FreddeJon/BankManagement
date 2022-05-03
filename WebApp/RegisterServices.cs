using System.Text;
using Application.Configurations;
using AzureSearch;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Persistence.Contracts;

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

        builder.Services.Configure<JwtOptions>(
            builder.Configuration.GetSection(nameof(JwtOptions)));


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
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<IUserService, UserService>();

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

                    ValidAudience = builder.Configuration["JwtOptions:ValidAudience"],
                    ValidIssuer = builder.Configuration["JwtOptions:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]))
                };
            });
    }
}