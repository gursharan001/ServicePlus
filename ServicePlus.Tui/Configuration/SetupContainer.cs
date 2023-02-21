using Lamar;
using Microsoft.Extensions.Configuration;
using ServicePlus.Core.Configuration;
using ServicePlus.ServiceControl.Configuration;

namespace ServicePlus.Tui.Configuration;

public static class SetupContainer
{
    public static void InitializeIoc(this ServiceRegistry serviceRegistry, IConfiguration configuration)
    {
        serviceRegistry.AddRange(new MartenRegistry(configuration));
        serviceRegistry.IncludeRegistry<ServiceControlRegistry>();
    }
}