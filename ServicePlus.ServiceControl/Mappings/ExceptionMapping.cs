
using Exception = ServicePlus.Core.Entities.Exception;

namespace ServicePlus.ServiceControl.Mappings;

public static class ExceptionMapping
{
    public static Exception ToEntity(this ServiceControlClient.Types.Exception exception)
    {
        return new Exception
        {
            ExceptionType = exception.ExceptionType,
            Message = exception.Message,
            Source = exception.Source,
            StackTrace = exception.StackTrace
        };
    }
}