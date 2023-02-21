using ServicePlus.Core.Entities;

namespace ServicePlus.ServiceControl.Mappings;

public static class HeaderKeyValueMapping
{
    public static HeaderKeyValue ToEntity(this ServiceControlClient.Types.HeaderKeyValue headerKeyValue)
    {
        return new HeaderKeyValue
        {
            Key = headerKeyValue.Key,
            Value = headerKeyValue.Value
        };
    }
}