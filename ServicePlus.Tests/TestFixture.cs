using System;
using System.Threading.Tasks;
using System.Xml.XPath;
using Lamar;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace ServicePlus.Tests;

public static class TestFixture
{
    public static async Task<T> ArrangeOnDocumentSessionAsync<T>(IContainer container,
        Func<IDocumentSession, Task<T>> arrange)
    {
        using var ns = container.CreateScope();
        var documentStore = ns.ServiceProvider.GetRequiredService<IDocumentStore>();
        await using var documentSession = documentStore.OpenSession();
        var result = await arrange(documentSession).ConfigureAwait(false);
        await documentSession.SaveChangesAsync().ConfigureAwait(false);
        return result;
    }
    
    public static async Task ArrangeOnDocumentSessionAsync(IContainer container,
        Func<IDocumentSession, Task> arrange)
    {
        using var ns = container.CreateScope();
        var documentStore = ns.ServiceProvider.GetRequiredService<IDocumentStore>();
        await using var documentSession = documentStore.OpenSession();
        await arrange(documentSession).ConfigureAwait(false);
        await documentSession.SaveChangesAsync().ConfigureAwait(false);
    }
    
    public static async Task ActAsync(IContainer container, 
        Func<IServiceProvider, Task> act, 
        Action<ServiceRegistry>? containerSetup = null)
    {
        var actualContainer = containerSetup != null
            ? AssemblySetupFixture.TestFixtureConfigureContainer(containerSetup)
            : container;
        using var ns = actualContainer.CreateScope();
        await act(ns.ServiceProvider).ConfigureAwait(false);
    }
    
    public static async Task<T> ActAsync<T>(IContainer container, 
        Func<IServiceProvider, Task<T>> act, 
        Action<ServiceRegistry>? containerSetup = null)
    {
        var actualContainer = containerSetup != null
            ? AssemblySetupFixture.TestFixtureConfigureContainer(containerSetup)
            : container;
        using var ns = actualContainer.CreateScope();
        return await act(ns.ServiceProvider).ConfigureAwait(false);
    }

    public static async Task AssertOnDocumentSessionAsync(IContainer container,
        Func<IQuerySession, Task> act)
    {
        using var ns = container.CreateScope();
        var documentStore = ns.ServiceProvider.GetRequiredService<IDocumentStore>();
        await using var documentSession = documentStore.QuerySession();
        await act(documentSession).ConfigureAwait(false);
    }
}