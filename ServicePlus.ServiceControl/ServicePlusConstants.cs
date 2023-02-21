namespace ServicePlus.ServiceControl;

internal class ServicePlusConstants
{
    public static readonly Uri PrdUrl = new("https://servicepulse.mondialevgl.com");
    public static readonly Uri QasUrl = new("https://servicepulse-qas.visaonetrack.com");
    public static readonly Uri LocalUrl = new("http://localhost:33333");

    public readonly string[] MessageHeaders = 
    {
        "NServiceBus.MessageId",
        "NServiceBus.CorrelationId",
        "NServiceBus.MessageIntent",
        "NServiceBus.Version",
        "NServiceBus.TimeSent",
        "NServiceBus.ContentType",
        "NServiceBus.EnclosedMessageTypes",
        "WinIdName",
        "NServiceBus.RelatedTo",
        "NServiceBus.ConversationId",
        "CorrId",
        "NServiceBus.OriginatingMachine",
        "NServiceBus.OriginatingEndpoint",
        "NServiceBus.ReplyToAddress",
        "NServiceBus.ExceptionInfo.ExceptionType",
        "NServiceBus.ExceptionInfo.InnerExceptionType",
        "NServiceBus.ExceptionInfo.HelpLink",
        "NServiceBus.ExceptionInfo.Message",
        "NServiceBus.ExceptionInfo.Source",
        "NServiceBus.ExceptionInfo.StackTrace",
        "NServiceBus.FailedQ",
        "NServiceBus.TimeOfFailure",
        "NServiceBus.Retries.Timestamp",
        "NServiceBus.Timeout.RouteExpiredTimeoutTo",
        "NServiceBus.Timeout.Expire",
        "NServiceBus.RelatedToTimeoutId",

        "$.diagnostics.originating.hostid",        
        "$.diagnostics.hostid",
        "$.diagnostics.hostdisplayname",

        "SignalRConnectionId",
        "UserId",
        "UserRef",
        "CommandSentUtc",
    };

    public const string TotalCountHeader = "Total-Count";
}