using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using ServicePlus.Core.Entities;
using Marten;
using static ServicePlus.Tests.TestFixtureExtensions;

namespace ServicePlus.Tests.DalTest;

[TestFixture]
public class LocalErrorMessageDalTest
{
    [SetUp]
    public void Setup()
    {
        ArrangeOnDocumentSessionAsync(s =>
        {
            s.DeleteWhere<LocalErrorMessage>(_ => true);
            return Task.CompletedTask;
        });
    }
    
    [TestCase("TestData/ErrorMessages/TwoErrorMessages.json")]
    public async Task CanSaveAndRead(string fileName)
    {
        var filePath = $"{TestContext.CurrentContext.TestDirectory}/{fileName}";
        var jsonText = File.ReadAllText(filePath);

        var errorMessages = JsonConvert.DeserializeObject<LocalErrorMessage[]>(jsonText);

        await ArrangeOnDocumentSessionAsync(s =>
        {
            s.Store(errorMessages!);
            return Task.CompletedTask;
        });

        await AssertOnDocumentSessionAsync(async s =>
        {
            var persisted = await s.Query<LocalErrorMessage>().ToListAsync();
            persisted.Should().BeEquivalentTo(errorMessages);
        });
    }
}