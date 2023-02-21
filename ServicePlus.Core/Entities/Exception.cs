namespace ServicePlus.Core.Entities;

public record Exception : IEntity
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

    public string ExceptionType { get; init; }
    public string Message { get; init; }
    public string Source { get; init; }
    public string StackTrace { get; init; }
}