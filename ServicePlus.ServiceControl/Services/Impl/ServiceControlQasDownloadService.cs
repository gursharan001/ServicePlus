using Marten;
using ServicePlus.Core.Entities;
using ServicePlus.ServiceControl.ServiceControlClient;

namespace ServicePlus.ServiceControl.Services.Impl;

internal class ServiceControlQasDownloadService : ServiceControlDownloadService
{
    public ServiceControlQasDownloadService(IDocumentStore documentStore,
        ServiceControlQasClient serviceControlQasClient) 
        : base(documentStore, serviceControlQasClient)
    {
    }
    
    public Task DownloadErrorMessagesForRecoverabilityGroup(string recoverabilityGroupId, 
        int count,
        CancellationTokenSource cts)
    {
        return base.DownloadErrorMessagesForRecoverabilityGroup<QasErrorMessage>(recoverabilityGroupId, count, cts);
    }
}