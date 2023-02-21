using ServicePlus.Core.Entities;

namespace ServicePlus.ServiceControl.Mappings;

public static class ErrorMessageMapping
{
    public static ErrorMessage ToEntity<T>(this ServiceControlClient.Types.ErrorMessage errorMessage) where T : ErrorMessage, new()
    {
        return new T
        {
            Id = errorMessage.Id,
            MessageType = errorMessage.MessageType,
            TimeSent = errorMessage.TimeSent,
            IsSystemMessage = errorMessage.IsSystemMessage,
            Exception = errorMessage.Exception.ToEntity(),
            MessageId = errorMessage.MessageId,
            NumberOfProcessingAttempts = errorMessage.NumberOfProcessingAttempts,
            Status = errorMessage.Status,
            SendingEndpoint = errorMessage.SendingEndpoint.ToEntity(),
            ReceivingEndpoint = errorMessage.ReceivingEndpoint.ToEntity(),
            QueueAddress = errorMessage.QueueAddress,
            TimeOfFailure = errorMessage.TimeOfFailure,
            LastModified = errorMessage.LastModified,
            Edited = errorMessage.Edited,
            EditOf = errorMessage.EditOf
        };
    }
}