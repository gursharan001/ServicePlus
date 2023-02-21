using Marten;
using ServicePlus.Core.Entities;
using ServicePlus.ServiceControl.ServiceControlClient;

namespace ServicePlus.ServiceControl.Services.Impl;

internal class ServiceControlLocalDownloadService : ServiceControlDownloadService
{
    public ServiceControlLocalDownloadService(IDocumentStore documentStore,
        ServiceControlLocalClient serviceControlLocalClient) 
        : base(documentStore, serviceControlLocalClient)
    {
    }

    public Task DownloadErrorMessagesForRecoverabilityGroup(string recoverabilityGroupId, 
        int count,
        CancellationTokenSource cts)
    {
        return base.DownloadErrorMessagesForRecoverabilityGroup<LocalErrorMessage>(recoverabilityGroupId, count, cts);
    }
}