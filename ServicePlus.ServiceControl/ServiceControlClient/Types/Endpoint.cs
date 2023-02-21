using Newtonsoft.Json;

namespace ServicePlus.ServiceControl.ServiceControlClient.Types;

public record Endpoint : IServiceControlType
{
#pragma warning disable CS8618
    protected internal Endpoint(){}
#pragma warning restore CS8618

    public Endpoint(string name, string hostId, string host)
    {
        Name = name;
        HostId = hostId;
        Host = host;
    }

    [JsonProperty(PropertyName = "name")] public string Name { get; init; }
    [JsonProperty(PropertyName = "host_id")] public string HostId { get; init; }
    [JsonProperty(PropertyName = "host")] public string Host { get; init; }
}