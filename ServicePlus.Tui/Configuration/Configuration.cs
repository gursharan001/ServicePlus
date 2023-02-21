using Microsoft.Extensions.Configuration;

namespace ServicePlus.Tui.Configuration;

public static class Configuration
{
    public static IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        return configuration;
    }
}