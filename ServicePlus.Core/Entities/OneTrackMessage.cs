namespace ServicePlus.Core.Entities;

public record OneTrackMessage
{
    public string Id { get; init; }
    public string MessageType { get; init; }
    public string MessageBlob { get; init; }
}