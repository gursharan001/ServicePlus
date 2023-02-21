using ServicePlus.Core.Entities;
using ServicePlus.ServiceControl.ServiceControlClient;

namespace ServicePlus.ServiceControl.Services;

internal interface IServiceControlDownloadService
{
    Task<int?> GetUnResolvedErrorMessageCount(CancellationTokenSource cts);
    public Task<RecoverabilityGroup[]> GetUnResolvedErrorRecoverabilityGroups(
        RecoverabilityGroupType recoverabilityGroupType,
        CancellationTokenSource cts);

    Task DownloadErrorMessagesForRecoverabilityGroup<T>(
        string recoverabilityGroupId,
        int count,
        CancellationTokenSource cts) where T: ErrorMessage, new();

    Task Retry(string[] ids, CancellationTokenSource cts);

    Task DownloadMessageBlobsAsync<T>(string messageType, string[] messageIds , CancellationTokenSource cts)
        where T: OneTrackMessage, new();
}