using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ServicePlus.Core.Entities;
using ServicePlus.ServiceControl.Services.Impl;
using Marten;
using ServicePlus.ServiceControl.ServiceControlClient;
using static ServicePlus.Tests.TestFixtureExtensions;

namespace ServicePlus.Tests.Services;

[TestFixture]
[Explicit]
public class ServiceControlLocalDownloadServiceTests
{

    [Test]
    public async Task DownloadErrorMessagesForEndpoint()
    {
        var cts = new CancellationTokenSource();
        
        await ArrangeOnDocumentSessionAsync(s =>
        {
            s.DeleteWhere<LocalErrorMessage>(_ => true);
            return Task.CompletedTask;
        });
        
        var endpointRecoverabilityGroups = 
            await ActAsync(sp => sp.GetRequiredService<ServiceControlLocalDownloadService>()
            .GetUnResolvedErrorRecoverabilityGroups(RecoverabilityGroupType.EndpointName, cts));

        Console.WriteLine(endpointRecoverabilityGroups[0].Title);
        await ActAsync(sp => sp.GetRequiredService<ServiceControlLocalDownloadService>()
            .DownloadErrorMessagesForRecoverabilityGroup(endpointRecoverabilityGroups[0].Id,
                endpointRecoverabilityGroups[0].Count,
                cts)).ConfigureAwait(false);

        await AssertOnDocumentSessionAsync(async s =>
        {
            var errorMessagesCount = await s.Query<LocalErrorMessage>().CountAsync();
            errorMessagesCount.Should().Be(endpointRecoverabilityGroups[0].Count);
            Console.WriteLine(errorMessagesCount);
        });
    }

    [Test]
    public async Task RetryErrorMessages()
    {
        var cts = new CancellationTokenSource();
        var filterTimestamp = new DateTime(2022, 06, 11, 02, 27, 14, DateTimeKind.Utc);
        var retryIds = await ArrangeOnDocumentSessionAsync(s => s.Query<LocalErrorMessage>()
            .Where(x => x.TimeOfFailureOffset > filterTimestamp)
            .Select(x => x.Id)
            .ToListAsync());
        
        await ActAsync(sp => sp.GetRequiredService<ServiceControlLocalDownloadService>()
            .Retry(retryIds.ToArray(), cts));
    }
}