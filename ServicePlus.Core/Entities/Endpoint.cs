namespace ServicePlus.Core.Entities;

public record Endpoint : IEntity
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

    public string Name { get; init; }
    public string HostId { get; init; }
    public string Host { get; init; }
}