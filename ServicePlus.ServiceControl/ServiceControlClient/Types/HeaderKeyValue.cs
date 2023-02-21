using Newtonsoft.Json;

namespace ServicePlus.ServiceControl.ServiceControlClient.Types;

public record HeaderKeyValue : IServiceControlType
{
#pragma warning disable CS8618
    protected internal HeaderKeyValue(){}
#pragma warning restore CS8618
    public HeaderKeyValue(string key, string? value)
    {
        Key = key;
        Value = value;
    }

    [JsonProperty(PropertyName = "key")] public string Key { get; init; }
    [JsonProperty(PropertyName = "value")]public string? Value { get; init; }
}