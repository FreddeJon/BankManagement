using Application.Options;
using Shared;

namespace Application;

public static class RegisterApplicationServices
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {

        var sharedConfigurations = RegisterSharedSettings.GetSharedSettings();

        services.Configure<JwtOptions>(
            sharedConfigurations.GetSection("JwtOptions"));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}