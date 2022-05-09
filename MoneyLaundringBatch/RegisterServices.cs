using MoneyLaunderingBatch.Contracts;
using MoneyLaunderingBatch.Options;
using MoneyLaunderingBatch.Services;
using Shared;

namespace MoneyLaunderingBatch;

public static class RegisterServices
{
    public static IConfiguration ConfigureMoneyLaunderingBatch(this IServiceCollection services)
    {
        var sharedConfigurations = RegisterSharedSettings.GetSharedSettings();

        services.Configure<MoneyLaunderingEmailOptions>(
            sharedConfigurations.GetSection("MoneyLaunderingEmailOptions"));

        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<ILaunderingChecker, LaunderingChecker>();
        return sharedConfigurations;
    }
}

