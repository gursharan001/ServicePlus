using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ServicePlus.ServiceControl.ServiceControlClient;
using static ServicePlus.Tests.TestFixtureExtensions;

namespace ServicePlus.Tests.Services;

[TestFixture]
public class ServiceControlLocalClientTests
{
    [Test]
    public async Task GetUnResolvedErrorMessageCount()
    {
        var cts = new CancellationTokenSource();
        var count = await ActAsync(sp => sp
                                        .GetRequiredService<ServiceControlLocalClient>()
                                        .GetUnResolvedErrorMessageCountAsync(cts));
        count.Should().NotBeNull();
        Console.WriteLine(count);
    }

    [Test]
    public async Task GetUnResolvedErrorMessagesByPage()
    {
        var cts = new CancellationTokenSource();
        var errors = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlLocalClient>()
            .GetUnResolvedErrorMessagesByPageAsync(1, cts));

        errors.Should().NotBeEmpty();
    }

    [Test]
    public async Task GetUnResolvedErrorEndpointRecoverabilityGroups()
    {
        var cts = new CancellationTokenSource();
        var endpointRecoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlLocalClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.EndpointName, cts));

        endpointRecoverabilityGroups.Should().NotBeEmpty();
    }
    
    [Test]
    public async Task GetUnResolvedErrorMessagesByEndpointRecoverabilityGroupPage()
    {
        var cts = new CancellationTokenSource();
        var endpointRecoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlLocalClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.EndpointName, cts));
        endpointRecoverabilityGroups.Should().NotBeEmpty();
        
        var errors = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlLocalClient>()
            .GetUnResolvedErrorMessagesByRecoverabilityGroupPageAsync(endpointRecoverabilityGroups[0].Id, 1, cts));

        errors.Should().NotBeEmpty();
        errors.ToList()
            .ForEach(error => Console.WriteLine(error));
    }

    [Test]
    public async Task ArchiveTest()
    {
        var cts = new CancellationTokenSource();
        await ActAsync(sp => sp
            .GetRequiredService<ServiceControlLocalClient>()
            .ArchiveAsync(new[] { "e9e27aee-74ca-ceba-3bea-f6182dd3ac01" }, cts));
    }
}