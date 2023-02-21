using System;
using System.Threading.Tasks;
using Lamar;
using Marten;

namespace ServicePlus.Tests;

public static class TestFixtureExtensions
{
    public static Task<T> ArrangeOnDocumentSessionAsync<T>(Func<IDocumentSession, Task<T>> arrange)
    {
        return TestFixture.ArrangeOnDocumentSessionAsync(AssemblySetupFixture.TestFixtureContainer, 
            arrange);
    }
    
    public static Task ArrangeOnDocumentSessionAsync(Func<IDocumentSession, Task> arrange)
    {
        return TestFixture.ArrangeOnDocumentSessionAsync(AssemblySetupFixture.TestFixtureContainer, 
            arrange);
    }
    
    public static Task ActAsync(Func<IServiceProvider, Task> act, 
        Action<ServiceRegistry>? containerSetup = null)
    {
        return TestFixture.ActAsync(AssemblySetupFixture.TestFixtureContainer, 
            act, 
            containerSetup);
    }
    
    public static Task<T> ActAsync<T>(Func<IServiceProvider, Task<T>> act, 
        Action<ServiceRegistry>? containerSetup = null)
    {
        return TestFixture.ActAsync(AssemblySetupFixture.TestFixtureContainer, 
            act, 
            containerSetup);
    }

    public static Task AssertOnDocumentSessionAsync(Func<IQuerySession, Task> act)
    {
        return TestFixture.AssertOnDocumentSessionAsync(AssemblySetupFixture.TestFixtureContainer,
            act);
    }
}