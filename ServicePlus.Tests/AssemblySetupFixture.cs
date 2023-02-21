using System;
using Lamar;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using ServicePlus.Tui.Configuration;
using Configuration = ServicePlus.Tui.Configuration.Configuration;

namespace ServicePlus.Tests;

[SetUpFixture]
public static class AssemblySetupFixture
{
    public static IContainer TestFixtureContainer { get; } = CreateContainer(null);

    public static IContainer TestFixtureConfigureContainer(Action<ServiceRegistry> configureContainer)
        => CreateContainer(configureContainer);
    
    private static IContainer CreateContainer(Action<ServiceRegistry>? configureContainer)
    {
        var configuration = Configuration.BuildConfiguration();
        var serviceRegistry = new ServiceRegistry();
        serviceRegistry.InitializeIoc(configuration);
        serviceRegistry.For<IConfiguration>().Use(configuration);
        configureContainer?.Invoke(serviceRegistry);
        var c = new Container(serviceRegistry);
        return c;
    }
    
}