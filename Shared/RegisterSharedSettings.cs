using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Shared;

public static class RegisterSharedSettings
{
    public static IConfiguration GetSharedSettings()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var builder = new ConfigurationBuilder()
            .SetBasePath(assemblyPath)
            .AddJsonFile("sharedsettings.json", optional: false)
            .AddUserSecrets(assembly: Assembly.GetExecutingAssembly());


        IConfiguration configuration = builder.Build();

        return configuration;
    }
}