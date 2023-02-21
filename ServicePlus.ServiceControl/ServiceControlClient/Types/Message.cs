using Newtonsoft.Json;

namespace ServicePlus.ServiceControl.ServiceControlClient.Types;

public record Message : IServiceControlType
{
#pragma warning disable CS8618
    protected internal Message()
    {
    }
#pragma warning restore CS8618

    public Message(string id, 
        string messageId, 
        string messageType, 
        Endpoint sendingEndpoint,
        Endpoint receivingEndpoint, 
        DateTime timeSent, 
        DateTime processesAt, 
        TimeSpan criticalTime,
        TimeSpan processingTime, 
        TimeSpan deliveryTime, 
        bool isSystemMessage, 
        string conversationId,
        HeaderKeyValue[] headers, 
        string status, 
        string messageIntent, 
        string bodyUrl, 
        int bodySize, 
        string instanceId)
    {
        Id = id;
        MessageId = messageId;
        MessageType = messageType;
        SendingEndpoint = sendingEndpoint;
        ReceivingEndpoint = receivingEndpoint;
        TimeSent = timeSent;
        ProcessesAt = processesAt;
        CriticalTime = criticalTime;
        ProcessingTime = processingTime;
        DeliveryTime = deliveryTime;
        IsSystemMessage = isSystemMessage;
        ConversationId = conversationId;
        Headers = headers;
        Status = status;
        MessageIntent = messageIntent;
        BodyUrl = bodyUrl;
        BodySize = bodySize;
        InstanceId = instanceId;
    }

    [JsonProperty(PropertyName = "id")] public string Id { get; init; }
    [JsonProperty(PropertyName = "message_id")] public string MessageId { get; init; }
    [JsonProperty(PropertyName = "message_type")] public string MessageType { get; init; }
    [JsonProperty(PropertyName = "sending_endpoint")] public Endpoint SendingEndpoint { get; init; }
    [JsonProperty(PropertyName = "receiving_endpoint")] public Endpoint ReceivingEndpoint { get; init; }
    [JsonProperty(PropertyName = "time_sent")] public DateTime TimeSent { get; init; }
    [JsonProperty(PropertyName = "processed_at")] public DateTime ProcessesAt { get; init; }
    [JsonProperty(PropertyName = "critical_time")] public TimeSpan CriticalTime { get; init; }
    [JsonProperty(PropertyName = "processing_time")] public TimeSpan ProcessingTime { get; init; }
    [JsonProperty(PropertyName = "delivery_time")] public TimeSpan DeliveryTime { get; init; }
    [JsonProperty(PropertyName = "is_system_message")] public bool IsSystemMessage { get; init; }
    [JsonProperty(PropertyName = "conversation_id")] public string ConversationId { get; init; }
    [JsonProperty(PropertyName = "headers")] public HeaderKeyValue[] Headers { get; init; }
    [JsonProperty(PropertyName = "status")] public string Status { get; init; }
    [JsonProperty(PropertyName = "message_intent")] public string MessageIntent { get; init; }
    [JsonProperty(PropertyName = "body_url")] public string BodyUrl { get; init; }
    [JsonProperty(PropertyName = "body_size")] public int BodySize { get; init; }
    [JsonProperty(PropertyName = "instance_id")] public string InstanceId { get; init; }
}