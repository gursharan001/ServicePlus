using Newtonsoft.Json;

namespace ServicePlus.ServiceControl.Services;

internal record RecoverabilityGroup
{
    [JsonProperty("id")] public string Id { get; init; } = Guid.NewGuid().ToString();
    #nullable disable
    [JsonProperty("title")] public string Title { get; init; }
    [JsonProperty("type")] public string Type { get; init; }
    [JsonProperty("count")] public int Count { get; init; }
    [JsonProperty("first")] public DateTime First { get; init; }
    [JsonProperty("last")] public DateTime Last { get; init; }
    [JsonProperty("operation_status")] public string OperationStatus { get; init; }
    [JsonProperty("operation_progress")] public decimal OperationProgress { get; init; }
    [JsonProperty("need_user_acknowledgement")] public bool NeedUserAcknowledgement { get; init; }
    #nullable enable
}