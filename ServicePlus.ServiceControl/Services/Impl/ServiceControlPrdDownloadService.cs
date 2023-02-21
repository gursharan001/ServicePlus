using Marten;
using ServicePlus.Core.Entities;
using ServicePlus.ServiceControl.ServiceControlClient;

namespace ServicePlus.ServiceControl.Services.Impl;

internal class ServiceControlPrdDownloadService : ServiceControlDownloadService
{
    public ServiceControlPrdDownloadService(IDocumentStore documentStore,
        ServiceControlPrdClient serviceControlPrdClient) 
        : base(documentStore, serviceControlPrdClient)
    {
    }
    
    public Task DownloadErrorMessagesForRecoverabilityGroup(string recoverabilityGroupId, 
        int count,
        CancellationTokenSource cts)
    {
        return base.DownloadErrorMessagesForRecoverabilityGroup<PrdErrorMessage>(recoverabilityGroupId, count, cts);
    }
}