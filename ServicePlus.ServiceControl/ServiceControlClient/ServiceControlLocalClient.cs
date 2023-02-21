namespace ServicePlus.ServiceControl.ServiceControlClient;

internal class ServiceControlLocalClient : ServiceControlHttpClient
{
    public ServiceControlLocalClient(HttpClient httpClient) : base(httpClient)
    {
        HttpClient.BaseAddress = ServicePlusConstants.LocalUrl;
    }
}