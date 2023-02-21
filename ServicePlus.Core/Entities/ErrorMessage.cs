using Newtonsoft.Json;

namespace ServicePlus.Core.Entities;

public record ErrorMessage : IEntity
{
#pragma warning disable CS8618
    protected internal ErrorMessage()
    {
    }
#pragma warning restore CS8618

    public ErrorMessage(string id,
        string messageType, 
        DateTime timeSent,
        bool isSystemMessage, 
        Exception exception,
        string messageId, 
        int numberOfProcessingAttempts, 
        string status, 
        Endpoint sendingEndpoint,
        Endpoint receivingEndpoint, 
        string queueAddress, 
        DateTime timeOfFailure, 
        DateTime lastModified, 
        bool edited,
        string editOf)
    {
        Id = id;
        MessageType = messageType;
        TimeSent = timeSent;
        IsSystemMessage = isSystemMessage;
        Exception = exception;
        MessageId = messageId;
        NumberOfProcessingAttempts = numberOfProcessingAttempts;
        Status = status;
        SendingEndpoint = sendingEndpoint;
        ReceivingEndpoint = receivingEndpoint;
        QueueAddress = queueAddress;
        TimeOfFailure = timeOfFailure;
        LastModified = lastModified;
        Edited = edited;
        EditOf = editOf;
    }

    public string Id { get; init; }
    public string MessageType { get; init; }
    public DateTime TimeSent { get; init; }
    public bool IsSystemMessage { get; init; }
    public Exception Exception { get; init; }
    public string MessageId { get; init; }
    public int NumberOfProcessingAttempts { get; init; }
    public string Status { get; init; }
    public Endpoint SendingEndpoint { get; init; }
    public Endpoint ReceivingEndpoint { get; init; }
    public string QueueAddress { get; init; }
    public DateTime TimeOfFailure { get; init; }
    [JsonIgnore] public DateTimeOffset TimeOfFailureOffset => DateTime.SpecifyKind(TimeOfFailure, DateTimeKind.Utc);
    public DateTime LastModified { get; init; }
    public bool Edited { get; init; }
    public string EditOf { get; init; }
}