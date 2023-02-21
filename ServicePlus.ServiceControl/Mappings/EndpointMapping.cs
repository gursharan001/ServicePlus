using ServicePlus.Core.Entities;

namespace ServicePlus.ServiceControl.Mappings;

public static class EndpointMapping
{
    public static Endpoint ToEntity(this ServiceControlClient.Types.Endpoint endpoint)
    {
        return new Endpoint
        {
            Host = endpoint.Host,
            HostId = endpoint.HostId,
            Name = endpoint.Name
        };
    }
}