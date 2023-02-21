namespace ServicePlus.ServiceControl.ServiceControlClient;

internal class ServiceControlQasClient : ServiceControlHttpClient
{
    public ServiceControlQasClient(HttpClient httpClient) : base(httpClient)
    {
        HttpClient.BaseAddress = ServicePlusConstants.QasUrl;
    }
}