using Newtonsoft.Json;

namespace ServicePlus.ServiceControl.ServiceControlClient.Types;

public record ErrorMessage : IServiceControlType
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

    [JsonProperty(PropertyName = "id")] public string Id { get; init; }
    [JsonProperty(PropertyName = "message_type")] public string MessageType { get; init; }
    [JsonProperty(PropertyName = "time_sent")] public DateTime TimeSent { get; init; }
    [JsonProperty(PropertyName = "is_system_message")] public bool IsSystemMessage { get; init; }
    [JsonProperty(PropertyName = "exception")] public Exception Exception { get; init; }
    [JsonProperty(PropertyName = "message_id")] public string MessageId { get; init; }
    [JsonProperty(PropertyName = "number_of_processing_attempts")] public int NumberOfProcessingAttempts { get; init; }
    [JsonProperty(PropertyName = "status")] public string Status { get; init; }
    [JsonProperty(PropertyName = "sending_endpoint")] public Endpoint SendingEndpoint { get; init; }
    [JsonProperty(PropertyName = "receiving_endpoint")] public Endpoint ReceivingEndpoint { get; init; }
    [JsonProperty(PropertyName = "queue_address")] public string QueueAddress { get; init; }
    [JsonProperty(PropertyName = "time_of_failure")] public DateTime TimeOfFailure { get; init; }
    [JsonIgnore] public DateTimeOffset TimeOfFailureOffset => DateTime.SpecifyKind(TimeOfFailure, DateTimeKind.Utc);
    [JsonProperty(PropertyName = "last_modified")] public DateTime LastModified { get; init; }
    [JsonProperty(PropertyName = "edited")] public bool Edited { get; init; }
    [JsonProperty(PropertyName = "edit_of")] public string EditOf { get; init; }
}