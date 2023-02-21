using System.Net.Http.Json;
using Newtonsoft.Json;
using ServicePlus.ServiceControl.ServiceControlClient.Types;
using ServicePlus.ServiceControl.Services;

namespace ServicePlus.ServiceControl.ServiceControlClient;

internal abstract class ServiceControlHttpClient
{
    protected readonly HttpClient HttpClient;

    public ServiceControlHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public static readonly Dictionary<RecoverabilityGroupType, string> RecoverabilityGroupUrlFragment
        = new()
        {
            { RecoverabilityGroupType.EndpointName, "Endpoint%20Name" },
            { RecoverabilityGroupType.EndpointAddress, "Endpoint%20Address" },
            { RecoverabilityGroupType.EndpointInstance, "Endpoint%20Instance" },
            { RecoverabilityGroupType.MessageType, "Message%20Type" },
            { RecoverabilityGroupType.ExceptionTypeAndStackTrace, "Exception%20Type%20And%20Stack%20Trace" },
        };
    public string UnresolvedErrorMessageUrl => "/api/errors?status=unresolved";
    public string UnresolvedErrorMessagePageUrl => "/api/errors?status=unresolved&page={0}&sort=time_of_failure&direction=desc";
    public string UnresolvedErrorRecoverabilityGroupUrl => "/api/recoverability/groups/{0}?classifierFilter=undefined";
    public string UnresolvedErrorMessageRecoverabilityGroupPageUrl => "/api/recoverability/groups/{0}/errors?page={1}&sort=time_of_failure&status=unresolved&direction=desc";
    public string UnresolvedErrorRetryUrl => "/api/errors/retry";
    public string MessageBodyUrl => "/api/messages/{0}/body";
    public string ArchiveUrl => "/api/errors/archive";
    public async Task<int?> GetUnResolvedErrorMessageCountAsync(
        CancellationTokenSource cts)
    {
        var msg = new HttpRequestMessage(HttpMethod.Head, UnresolvedErrorMessageUrl);
        var httpMessageResponse = await HttpClient.SendAsync(msg, cts.Token).ConfigureAwait(false);
        var totalCountHeader = httpMessageResponse
            .Headers
            .GetValues(ServicePlusConstants.TotalCountHeader)
            .SingleOrDefault();
        if (string.IsNullOrWhiteSpace(totalCountHeader)) return null;
        return int.Parse(totalCountHeader);
    }

    public async Task<ErrorMessage[]> GetUnResolvedErrorMessagesByPageAsync(
        int pageNumber,
        CancellationTokenSource cts)
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, string.Format(UnresolvedErrorMessagePageUrl, pageNumber));
        var httpMessageResponse = await HttpClient.SendAsync(msg, cts.Token).ConfigureAwait(false);
        var jsonResponse = await httpMessageResponse.Content.ReadAsStringAsync(cts.Token).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(jsonResponse)) return Array.Empty<ErrorMessage>();
        return JsonConvert.DeserializeObject<ErrorMessage[]>(jsonResponse)!;
    }

    public async Task<RecoverabilityGroup[]> GetUnResolvedErrorRecoverabilityGroupsAsync(
        RecoverabilityGroupType recoverabilityGroupType,
        CancellationTokenSource cts)
    {
        var recoverabilityGroupUrl = string.Format(UnresolvedErrorRecoverabilityGroupUrl,
            RecoverabilityGroupUrlFragment[recoverabilityGroupType]);
        var msg = new HttpRequestMessage(HttpMethod.Get, recoverabilityGroupUrl);
        var httpMessageResponse = await HttpClient.SendAsync(msg, cts.Token).ConfigureAwait(false);
        var jsonResponse = await httpMessageResponse.Content.ReadAsStringAsync(cts.Token).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(jsonResponse)) return Array.Empty<RecoverabilityGroup>();
        return JsonConvert.DeserializeObject<RecoverabilityGroup[]>(jsonResponse)!;
    }

    public async Task<ErrorMessage[]> GetUnResolvedErrorMessagesByRecoverabilityGroupPageAsync(
        string endpointRecoverabilityGroup, 
        int pageNumber,
        CancellationTokenSource cts)
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, 
            string.Format(UnresolvedErrorMessageRecoverabilityGroupPageUrl, endpointRecoverabilityGroup, pageNumber));
        var httpMessageResponse = await HttpClient.SendAsync(msg, cts.Token).ConfigureAwait(false);
        var jsonResponse = await httpMessageResponse.Content.ReadAsStringAsync(cts.Token).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(jsonResponse)) return Array.Empty<ErrorMessage>();
        return JsonConvert.DeserializeObject<ErrorMessage[]>(jsonResponse)!;
    }

    public async Task RetryAsync(string[] ids, CancellationTokenSource cts)
    {
        var httpMessageResponse = await HttpClient
            .PostAsJsonAsync(UnresolvedErrorRetryUrl, ids, cts.Token)
            .ConfigureAwait(false);
        if (!httpMessageResponse.IsSuccessStatusCode)
        {
            throw new System.Exception($"Error retrying - {httpMessageResponse.ReasonPhrase}");
        }
    }

    public async Task<string> GetMessageBlobAsync(string messageId, CancellationTokenSource cts)
    {
        var bodyUrl = string.Format(MessageBodyUrl, messageId);
        var body = await HttpClient
            .GetStringAsync(bodyUrl, cts.Token)
            .ConfigureAwait(false);
        return body;
    }
    
    public async Task ArchiveAsync(string[] ids, CancellationTokenSource cts)
    {
        var content = JsonContent.Create(ids);
        var httpMessageResponse = await HttpClient
            .PatchAsync(ArchiveUrl, content, cts.Token)
            .ConfigureAwait(false);
        if (!httpMessageResponse.IsSuccessStatusCode)
        {
            throw new System.Exception($"Error archiving - {httpMessageResponse.ReasonPhrase}");
        }
    }
}