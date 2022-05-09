using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Shared;

public static class RegisterSharedSettings
{
    public static IConfiguration GetSharedSettings()
    {
        var path = Directory.GetCurrentDirectory().Contains("WebApp")
            ? Directory.GetParent(Directory.GetCurrentDirectory()) + @"\Shared"
            : Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent + @"\Shared";

        //var path = Directory.GetParent(Directory.GetCurrentDirectory()) + @"\Shared";

        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("sharedsettings.json", optional: false)
            .AddUserSecrets(assembly: Assembly.GetExecutingAssembly());


        IConfiguration configuration = builder.Build();

        return configuration;
    }
}