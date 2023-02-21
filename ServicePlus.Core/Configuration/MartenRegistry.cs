using Lamar;
using Marten;
using Microsoft.Extensions.Configuration;
using ServicePlus.Core.Entities;
using Weasel.Core;

namespace ServicePlus.Core.Configuration;

public class MartenRegistry : ServiceRegistry
{
    public MartenRegistry(IConfiguration configuration)
    {
        ForSingletonOf<IDocumentStore>().Use(c =>
        {
            return DocumentStore.For(options =>
            {
                options.Connection(configuration.GetConnectionString("ServicePlus"));
                options.AutoCreateSchemaObjects = AutoCreate.All;
                
                options.Schema.For<ErrorMessage>()
                    .GinIndexJsonData()
                    .Duplicate(x => x.TimeOfFailureOffset)
                    .AddSubClass<LocalErrorMessage>()
                    .AddSubClass<QasErrorMessage>()
                    .AddSubClass<PrdErrorMessage>();

                options.Schema.For<OneTrackMessage>()
                    .GinIndexJsonData()
                    .AddSubClass<LocalOneTrackMessage>()
                    .AddSubClass<QasOneTrackMessage>()
                    .AddSubClass<PrdOneTrackMessage>();
            });
        });
    }
}