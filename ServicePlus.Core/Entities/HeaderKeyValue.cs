namespace ServicePlus.Core.Entities;

public record HeaderKeyValue : IEntity
{
#pragma warning disable CS8618
    protected internal HeaderKeyValue(){}
#pragma warning restore CS8618
    public HeaderKeyValue(string key, string? value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; init; }
    public string? Value { get; init; }
}