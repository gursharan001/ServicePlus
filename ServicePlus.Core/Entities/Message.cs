namespace ServicePlus.Core.Entities;

public record Message : IEntity
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

    public string Id { get; init; }
    public string MessageId { get; init; }
    public string MessageType { get; init; }
    public Endpoint SendingEndpoint { get; init; }
    public Endpoint ReceivingEndpoint { get; init; }
    public DateTime TimeSent { get; init; }
    public DateTime ProcessesAt { get; init; }
    public TimeSpan CriticalTime { get; init; }
    public TimeSpan ProcessingTime { get; init; }
    public TimeSpan DeliveryTime { get; init; }
    public bool IsSystemMessage { get; init; }
    public string ConversationId { get; init; }
    public HeaderKeyValue[] Headers { get; init; }
    public string Status { get; init; }
    public string MessageIntent { get; init; }
    public string BodyUrl { get; init; }
    public int BodySize { get; init; }
    public string InstanceId { get; init; }
}