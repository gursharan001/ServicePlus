using ServicePlus.Core.Entities;

namespace ServicePlus.ServiceControl.Mappings;

public static class MessageMapping
{
    public static Message ToEntity(this ServiceControlClient.Types.Message message)
    {
        return new Message
        {
            Id = message.Id,
            MessageId = message.MessageId,
            MessageType = message.MessageType,
            SendingEndpoint = message.SendingEndpoint.ToEntity(),
            ReceivingEndpoint = message.ReceivingEndpoint.ToEntity(),
            TimeSent = message.TimeSent,
            ProcessesAt = message.ProcessesAt,
            CriticalTime = message.CriticalTime,
            ProcessingTime = message.ProcessingTime,
            DeliveryTime = message.DeliveryTime,
            IsSystemMessage = message.IsSystemMessage,
            ConversationId = message.ConversationId,
            Headers = message.Headers.Select(x => x.ToEntity()).ToArray(),
            Status = message.Status,
            MessageIntent = message.MessageIntent,
            BodyUrl = message.BodyUrl,
            BodySize = message.BodySize,
            InstanceId = message.InstanceId
        };
    }
}