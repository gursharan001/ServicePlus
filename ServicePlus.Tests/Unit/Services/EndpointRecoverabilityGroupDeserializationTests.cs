using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using FluentAssertions;
using NUnit.Framework;
using ServicePlus.ServiceControl.Services;

namespace ServicePlus.Tests.Unit.Services;

[TestFixture]
public class EndpointRecoverabilityGroupDeserializationTests
{
    [TestCase("TestData/RecoverabilityGroups/EndpointRecoverabilityGroups.json")]
    public void Deserializes(string fileName)
    {
        var filePath = $"{TestContext.CurrentContext.TestDirectory}/{fileName}";
        var jsonText = File.ReadAllText(filePath);

        var result = JsonConvert.DeserializeObject<RecoverabilityGroup[]>(jsonText);

        result.Should().NotBeNull();
        result!.Length.Should().Be(1);
        result.Should()
            .BeEquivalentTo(new []
            {
                new RecoverabilityGroup
                {
                    Id = "b30d9bb2-e914-2346-e867-9d0836696690",
                    Title = "devnsbserver@GSINGHDEVW10",
                    Type = "Endpoint Address",
                    Count = 19,
                    First = DateTime.Parse("2022-06-06T07:23:37.245057Z", null, DateTimeStyles.RoundtripKind),
                    Last = DateTime.Parse("2022-06-11T02:30:45.871961Z", null, DateTimeStyles.RoundtripKind),
                    OperationStatus = "None",
                    OperationProgress = 0.0m,
                    NeedUserAcknowledgement = false
                }
            });
    }
}