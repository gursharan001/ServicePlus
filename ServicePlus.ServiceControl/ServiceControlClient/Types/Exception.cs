using Newtonsoft.Json;

namespace ServicePlus.ServiceControl.ServiceControlClient.Types;

public record Exception : IServiceControlType
{
#pragma warning disable CS8618
    protected internal Exception(){}
#pragma warning restore CS8618
    public Exception(string exceptionType, string message, string source, string stackTrace)
    {
        ExceptionType = exceptionType;
        Message = message;
        Source = source;
        StackTrace = stackTrace;
    }

    [JsonProperty(PropertyName = "exception_type")] public string ExceptionType { get; init; }
    [JsonProperty(PropertyName = "message")] public string Message { get; init; }
    [JsonProperty(PropertyName = "source")] public string Source { get; init; }
    [JsonProperty(PropertyName = "stack_trace")] public string StackTrace { get; init; }
}