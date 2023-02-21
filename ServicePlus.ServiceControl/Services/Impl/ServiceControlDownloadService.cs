using System.Threading.Channels;
using Marten;
using ServicePlus.Core.Entities;
using ServicePlus.ServiceControl.Mappings;
using ServicePlus.ServiceControl.ServiceControlClient;

namespace ServicePlus.ServiceControl.Services.Impl;

internal abstract class ServiceControlDownloadService : IServiceControlDownloadService
{
    private readonly ServiceControlHttpClient _serviceControlClient;
    private readonly IDocumentStore _documentStore;
    private readonly int numberOfWorkers = 5;
    private readonly int numberOfRetryWorkers = 1;
    private readonly int messagesPerPage = 50;
    private readonly int messagesPerRetryPage = 50;
    public ServiceControlDownloadService(IDocumentStore documentStore, 
        ServiceControlHttpClient serviceControlClient)
    {
        _documentStore = documentStore;
        _serviceControlClient = serviceControlClient;
    }

    public Task<int?> GetUnResolvedErrorMessageCount(CancellationTokenSource cts)
    {
        return _serviceControlClient.GetUnResolvedErrorMessageCountAsync(cts);
    }

    public Task<RecoverabilityGroup[]> GetUnResolvedErrorRecoverabilityGroups(
        RecoverabilityGroupType recoverabilityGroupType,
        CancellationTokenSource cts)
    {
        return _serviceControlClient.GetUnResolvedErrorRecoverabilityGroupsAsync(recoverabilityGroupType, cts);
    }

    public async Task DownloadErrorMessagesForRecoverabilityGroup<T>(
        string recoverabilityGroupId,
        int count,
        CancellationTokenSource cts) where T: ErrorMessage, new()
    {
        var workQueue = Channel.CreateUnbounded<int>();
        var workers = new List<Task>();
        for(var i=1; i<=numberOfWorkers; i++)
        {
            workers.Add(
                DownloadErrorMessagesForRecoverabilityGroupByPageAsync<T>(workQueue.Reader,
                    recoverabilityGroupId,
                    cts));
        }

        var numberOfPages = count / messagesPerPage + 1;
        foreach (var pageNumber in Enumerable.Range(1, numberOfPages ))
        {
            await workQueue.Writer.WriteAsync(pageNumber).ConfigureAwait(false);
        }

        workQueue.Writer.Complete();

        await Task.WhenAll(workers).ConfigureAwait(false);
    }

    private async Task DownloadErrorMessagesForRecoverabilityGroupByPageAsync<T>(ChannelReader<int> pageNumbers,
        string recoverabilityGroupId,
        CancellationTokenSource cts) where T: ErrorMessage, new()
    {
        try
        {
            while (await pageNumbers.WaitToReadAsync(cts.Token).ConfigureAwait(false))
            {
                if (pageNumbers.TryRead(out var pageNumber))
                {
                    var errorMessages = await _serviceControlClient
                        .GetUnResolvedErrorMessagesByRecoverabilityGroupPageAsync(
                            recoverabilityGroupId,
                            pageNumber,
                            cts).ConfigureAwait(false);
                    Console.WriteLine($"Downloaded page {pageNumber}");
                    if (errorMessages is { Length: 0 }) continue;
                    var entityErrorMessages = errorMessages
                        .Select(x => x.ToEntity<T>())
                        .ToArray();
                    await using var documentSession = _documentStore.LightweightSession();
                    documentSession.Store(entityErrorMessages);
                    await documentSession.SaveChangesAsync(cts.Token).ConfigureAwait(false);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // return
        }
    }
    
    public async Task Retry(string[] ids, CancellationTokenSource cts)
    {
        var workQueue = Channel.CreateUnbounded<RetryPage>();
        var workers = new List<Task>();
        for(var i=1; i<=numberOfRetryWorkers; i++)
        {
            workers.Add(RetryErrorMessagesAsync(workQueue.Reader, cts));
        }

        var numberOfPages = ids.Length / messagesPerPage + 1;
        foreach (var pageNumber in Enumerable.Range(1, numberOfPages))
        {
            var retryIds = ids
                .Skip((pageNumber - 1) * messagesPerRetryPage)
                .Take(messagesPerRetryPage)
                .ToArray();
            var retryPage = new RetryPage(pageNumber, retryIds);
            await workQueue.Writer.WriteAsync(retryPage).ConfigureAwait(false);
        }

        workQueue.Writer.Complete();

        await Task.WhenAll(workers).ConfigureAwait(false);
    }

    private async Task RetryErrorMessagesAsync(ChannelReader<RetryPage> retryPages, CancellationTokenSource cts)
    {
        try
        {
            while (await retryPages.WaitToReadAsync(cts.Token).ConfigureAwait(false))
            {
                if (retryPages.TryRead(out var retryPage))
                {
                    try
                    {
                        Console.WriteLine($"Retrying page {retryPage.PageNumber}");
                        await _serviceControlClient.RetryAsync(retryPage.Ids, cts).ConfigureAwait(false);
                    }
                    
                    catch (System.Exception ex) when (ex is not OperationCanceledException)
                    {
                        Console.WriteLine($"Error retrying page {retryPage.PageNumber} - {ex.ToString()}");
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // return
        }
    }

    private record RetryPage(int PageNumber, string[] Ids);
    
    public async Task DownloadMessageBlobsAsync<T>(string messageType, 
        string[] messageIds, 
        CancellationTokenSource cts) where T: OneTrackMessage, new()
    {
        var workQueue = Channel.CreateUnbounded<string>();
        var workers = new List<Task>();
        for(var i=1; i<=numberOfWorkers; i++)
        {
            workers.Add(
                DownloadMessageBlobAsync<T>(workQueue.Reader,
                    messageType,
                    cts));
        }
        
        foreach (var messageId in messageIds)
        {
            await workQueue.Writer.WriteAsync(messageId).ConfigureAwait(false);
        }

        workQueue.Writer.Complete();

        await Task.WhenAll(workers).ConfigureAwait(false);
    }

    private async Task DownloadMessageBlobAsync<T>(ChannelReader<string> messageIds, 
        string messageType,
        CancellationTokenSource cts) where T: OneTrackMessage, new()
    {
        try
        {
            while (await messageIds.WaitToReadAsync(cts.Token).ConfigureAwait(false))
            {
                if (messageIds.TryRead(out var messageId))
                {
                    try
                    {
                        var blob = await _serviceControlClient.GetMessageBlobAsync(messageId, cts).ConfigureAwait(false);
                        var oneTrackMessage = new T
                        {
                            Id = messageId,
                            MessageType = messageType,
                            MessageBlob = blob
                        };
                        await using var documentSession = _documentStore.LightweightSession();
                        documentSession.Store(oneTrackMessage);
                        await documentSession.SaveChangesAsync(cts.Token).ConfigureAwait(false);
                    }
                    
                    catch (System.Exception ex) when (ex is not OperationCanceledException)
                    {
                        Console.WriteLine($"Error download message {messageId} - {ex.ToString()}");
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // return
        }
    }
}

