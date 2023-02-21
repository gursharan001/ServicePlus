namespace ServicePlus.ServiceControl.ServiceControlClient;

internal class ServiceControlPrdClient : ServiceControlHttpClient
{
    public ServiceControlPrdClient(HttpClient httpClient) : base(httpClient)
    {
        HttpClient.BaseAddress = ServicePlusConstants.PrdUrl;
    }
}