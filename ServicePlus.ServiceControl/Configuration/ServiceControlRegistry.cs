using Lamar;
using Microsoft.Extensions.DependencyInjection;
using ServicePlus.ServiceControl.ServiceControlClient;
using ServicePlus.ServiceControl.Services;
using ServicePlus.ServiceControl.Services.Impl;

namespace ServicePlus.ServiceControl.Configuration;

public class ServiceControlRegistry : ServiceRegistry
{
    public ServiceControlRegistry()
    {
        this.AddHttpClient<ServiceControlLocalClient>();
        this.AddHttpClient<ServiceControlQasClient>();
        this.AddHttpClient<ServiceControlPrdClient>();
        this.AddScoped<ServiceControlLocalDownloadService>();
        this.AddScoped<ServiceControlQasDownloadService>();
        this.AddScoped<ServiceControlPrdDownloadService>();
    }
}