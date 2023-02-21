using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using CsvHelper;
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
public class ServiceControlPrdDownloadServiceTests
{
    [Test]
    public async Task GetEndpointRecoverabilityGroups()
    {
        var cts = new CancellationTokenSource();
        var endpointRecoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlPrdClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.EndpointName, cts));

        endpointRecoverabilityGroups.Should().NotBeEmpty();
        var notificationEndpoint = endpointRecoverabilityGroups.Single(x =>
            x.Title.StartsWith("prdnsbserver.notification", StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(notificationEndpoint);
    }

    [Test]
    public async Task GetMessageTypeRecoverabilityGroups()
    {
        var cts = new CancellationTokenSource();
        var recoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlPrdClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.MessageType, cts));

        recoverabilityGroups.Should().NotBeEmpty();
        var recoverabilityGroup = recoverabilityGroups.Single(x =>
            x.Title.StartsWith("Visa.Zaphod.PeriodicTasks.Commands.CargoServices.CreateCargoServicesShipmentCommand", 
                StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(recoverabilityGroup);
    }

    [Test]
    public async Task DownloadAllMessageTypeErrors()
    {
        var recoverabilityGroupTitle =
            "Visa.Zaphod.PeriodicTasks.Commands.CargoServices.CreateCargoServicesShipmentCommand";
        var cts = new CancellationTokenSource();

        var recoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlPrdClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.MessageType, cts));

        recoverabilityGroups.Should().NotBeEmpty();
        var recoverabilityGroup = recoverabilityGroups.Single(x =>
            x.Title.StartsWith(recoverabilityGroupTitle, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(recoverabilityGroup);
        
        await ArrangeOnDocumentSessionAsync(s =>
        {
            s.DeleteWhere<PrdErrorMessage>(x => x.ReceivingEndpoint.Name.StartsWith(recoverabilityGroupTitle));
            return Task.CompletedTask;
        });
        
        await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
            .DownloadErrorMessagesForRecoverabilityGroup(recoverabilityGroup.Id,
                recoverabilityGroup.Count,
                cts)).ConfigureAwait(false);

        await AssertOnDocumentSessionAsync(async s =>
        {
            var errorMessagesCount = await s.Query<PrdErrorMessage>()
                .Where(x => x.MessageType.StartsWith(recoverabilityGroupTitle, StringComparison.OrdinalIgnoreCase))
                .CountAsync();
            errorMessagesCount.Should().Be(recoverabilityGroup.Count);
            Console.WriteLine(errorMessagesCount);
        });
    }

    [Test]
    public async Task CheckCount()
    {
        await AssertOnDocumentSessionAsync(async s =>
        {
            var count = await s.Query<PrdErrorMessage>()
                .CountAsync(x => x.MessageType.Equals("Visa.Zaphod.PeriodicTasks.Commands.CargoServices.CreateCargoServicesShipmentCommand", StringComparison.OrdinalIgnoreCase));
                //.CountAsync();
            Console.WriteLine(count);
        });
    }

    [Test]
    public async Task Download_CreateCargoServicesShipmentCommand()
    {
        var cts = new CancellationTokenSource();
        var messageType = "Visa.Zaphod.PeriodicTasks.Commands.CargoServices.CreateCargoServicesShipmentCommand";
        var messageIds = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messageIds = await s.Query<PrdErrorMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.MessageId)
                .ToListAsync(cts.Token);
            return messageIds;
        });

        await ActAsync(c => c.GetRequiredService<ServiceControlPrdDownloadService>()
            .DownloadMessageBlobsAsync<PrdOneTrackMessage>(messageType, messageIds.ToArray(), cts));

        await AssertOnDocumentSessionAsync(async s =>
        {
            var persistedCount = await s.Query<PrdOneTrackMessage>()
                .CountAsync(x => x.MessageType == messageType);

            persistedCount.Should().Be(messageIds.Count);
        });
    }

    [Test]
    public async Task Search_CargoServicesShipmentCommand()
    {
        var cts = new CancellationTokenSource();
        var messageType = "Visa.Zaphod.PeriodicTasks.Commands.CargoServices.CreateCargoServicesShipmentCommand";
        
        await ArrangeOnDocumentSessionAsync(async s =>
        {
            var result = await s.Query<PrdOneTrackMessage>()
                .Where(x => x.MessageType == messageType)
                .Where(x => x.MessageBlob.Contains("SHMEL2320066"))
                .FirstOrDefaultAsync();

            result.Should().NotBeNull();
        });
    }

    [Test]
    public async Task Download_AddRelatedSupplierFromCargowiseCodeCommand_RecoverabilityGroup()
    {
        var recoverabilityGroupTitle =
            "Visa.Zaphod.ShippingDomain.Commands.Partnership.AddRelatedSupplierFromCargowiseCodeCommand";
        var cts = new CancellationTokenSource();

        var recoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlPrdClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.MessageType, cts));

        recoverabilityGroups.Should().NotBeEmpty();
        var recoverabilityGroup = recoverabilityGroups.Single(x =>
            x.Title.StartsWith(recoverabilityGroupTitle, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(recoverabilityGroup);
        
        await ArrangeOnDocumentSessionAsync(s =>
        {
            s.DeleteWhere<PrdErrorMessage>(x => x.ReceivingEndpoint.Name.StartsWith(recoverabilityGroupTitle));
            return Task.CompletedTask;
        });
        
        await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
            .DownloadErrorMessagesForRecoverabilityGroup(recoverabilityGroup.Id,
                recoverabilityGroup.Count,
                cts)).ConfigureAwait(false);

        await AssertOnDocumentSessionAsync(async s =>
        {
            var errorMessagesCount = await s.Query<PrdErrorMessage>()
                .Where(x => x.MessageType.StartsWith(recoverabilityGroupTitle, StringComparison.OrdinalIgnoreCase))
                .CountAsync();
            errorMessagesCount.Should().Be(recoverabilityGroup.Count);
            Console.WriteLine(errorMessagesCount);
        });
    }

    [Test]
    public async Task Download_PurchaseOrderAssignedToShipmentNsbEvent()
    {
        var cts = new CancellationTokenSource();
        var messageType = "NsbMessages.ShippingDomain.Events.PurchaseOrderAssignedToShipmentNsbEvent";
        var messageIds = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messageIds = await s.Query<PrdErrorMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.MessageId)
                .ToListAsync(cts.Token);
            return messageIds;
        });
        
        await ArrangeOnDocumentSessionAsync(s =>
        {
            s.DeleteWhere<PrdOneTrackMessage>(x => x.MessageType == messageType);
            return Task.CompletedTask;
        });

        await ActAsync(c => c.GetRequiredService<ServiceControlPrdDownloadService>()
            .DownloadMessageBlobsAsync<PrdOneTrackMessage>(messageType, messageIds.ToArray(), cts));

        await AssertOnDocumentSessionAsync(async s =>
        {
            var persistedCount = await s.Query<PrdOneTrackMessage>()
                .CountAsync(x => x.MessageType == messageType);

            persistedCount.Should().Be(messageIds.Count);
        });
    }
    
    [Test]
    public async Task AddRelatedSupplierFromCargowiseCodeCommand_ExceptionMessageCountCheck()
    {
        var cts = new CancellationTokenSource();
        var messageType = "Visa.Zaphod.ShippingDomain.Commands.Partnership.AddRelatedSupplierFromCargowiseCodeCommand";

        var messagesDataset = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messages = await s.Query<PrdOneTrackMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            var messagesDatasets = new List<DataSet>();
            foreach (var message in messages)
            {
                var ds = new DataSet();
                using var sr = new StringReader(message.MessageBlob);
                ds.ReadXml(sr);
                messagesDatasets.Add(ds);
            }

            var messagesDataset = new DataSet();
            foreach (var messageDataSet in messagesDatasets)
            {
                messagesDataset.Merge(messageDataSet);
            }

            return messagesDataset;
        });

        var cmds = messagesDataset.Tables["AddRelatedSupplierFromCargowiseCodeCommand"];
        var codes = cmds.AsEnumerable()
            .Select(r => r.Field<string>("SupplierCargowiseCode"))
            .Distinct()
            .ToList();
        codes.ForEach(Console.WriteLine);
    }

    [Test]
    public async Task RetryErrorMessages()
    {
        var cts = new CancellationTokenSource();
        var filterTimestamp = new DateTime(2022, 06, 11, 02, 27, 14, DateTimeKind.Utc);
        var retryIds = await ArrangeOnDocumentSessionAsync(s => s.Query<PrdErrorMessage>()
            .Where(x => x.TimeOfFailureOffset > filterTimestamp)
            .Select(x => x.Id)
            .ToListAsync());
        
        await ActAsync(sp => sp.GetRequiredService<ServiceControlLocalDownloadService>()
            .Retry(retryIds.ToArray(), cts));
    }
    
    [Test]
    public async Task DataSet_Test()
    {
        var sw = new Stopwatch();
        var messageType = "Visa.Zaphod.PeriodicTasks.Commands.CargoServices.CreateCargoServicesShipmentCommand";
        var messagesDataset = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messages = await s.Query<PrdOneTrackMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            var messagesDatasets = new List<DataSet>();
            sw.Start();
            foreach (var message in messages)
            {
                var ds = new DataSet();
                using var sr = new StringReader(message.MessageBlob);
                ds.ReadXml(sr);
                messagesDatasets.Add(ds);
            }
            sw.Stop();
            Console.WriteLine($"Time taken to read datasets: {sw.Elapsed.Seconds}");

            var messagesDataset = new DataSet();
            sw.Restart();
            foreach (var messageDataSet in messagesDatasets)
            {
                messagesDataset.Merge(messageDataSet);
            }
            sw.Stop();
            Console.WriteLine($"Time taken to merge datasets: {sw.Elapsed.Seconds}");
            
            return messagesDataset;
        });

        var csOrders = messagesDataset.Tables["CreateCargoServicesShipmentCommand"];
        
        sw.Restart();
        var events = csOrders.AsEnumerable()
            .Select(x => new {
                PurchaseOrderId = x.Field<string>("PurchaseOrderId"),
                ShipmentId = x.Field<string>("ShipmentId"),
                CustomerId = x.Field<string>("CustomerId"),
                CustomerPo = x.Field<string>("CustomerPo")
            })
            .ToList();
        sw.Stop();
        Console.WriteLine($"Time taken to query datasets: {sw.Elapsed.Seconds}");

        using (var writer = new StreamWriter("E:\\Temp\\ServicePlus\\PurchaseOrderAssignedToShipmentNsbEvent.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(events);
        }
    }

    [TestCase("TestData/Prd/MessageIdsToArchive.txt")]
    public async Task Archive_Messages_Test(string filename)
    {
        var filepath = $"{TestContext.CurrentContext.TestDirectory}/{filename}";
        var shipmentIds = File.ReadAllLines(filepath);
        var messageType = "NsbMessages.ShippingDomain.Events.PurchaseOrderAssignedToShipmentNsbEvent";

        var messagesDataset = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messages = await s.Query<PrdOneTrackMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
           
            var messagesDatasets = new List<DataSet>();
            foreach (var message in messages)
            {
                var ds = new DataSet();
                using var sr = new StringReader(message.MessageBlob);
                ds.ReadXml(sr);
                var evtTable = ds.Tables["PurchaseOrderAssignedToShipmentNsbEvent"];
                var idColumn = new DataColumn();
                idColumn.DataType = typeof(string);
                idColumn.ColumnName = "OneTrackMessageId";

                evtTable.Columns.Add(idColumn);
                DataRow row = evtTable.Rows[0];
                row["OneTrackMessageId"] = message.Id;
                
                messagesDatasets.Add(ds);
            }

            var messagesDataset = new DataSet();
            foreach (var messageDataSet in messagesDatasets)
            {
                messagesDataset.Merge(messageDataSet);
            }
            return messagesDataset;
        });
        
        var events = messagesDataset.Tables["PurchaseOrderAssignedToShipmentNsbEvent"];
        var oneTrackMessageIds = events.AsEnumerable()
            .Where(r => shipmentIds.Contains(r.Field<string>("ShipmentId")))
            .Select(r => r.Field<string>("OneTrackMessageId"))
            .ToArray();

        var cts = new CancellationTokenSource();
        
        var errorMessageIds = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var errorMessages = await s.Query<PrdErrorMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync(cts.Token);

            return errorMessages
                .Where(errorMessage => oneTrackMessageIds.Contains(errorMessage.MessageId))
                .Select(errorMessage => errorMessage.Id)
                .ToArray();
        });
        
        var pageSize = 100;
        var numberOfPages = errorMessageIds.Length / pageSize;
        
        for (var pageNumber=  1; pageNumber <= numberOfPages; pageNumber++)
        {
            var spanStart = GetSpanStart(pageNumber, pageSize);
            var spanLength = GetSpanLength(pageNumber, numberOfPages, pageSize, errorMessageIds.Length);
            var messagesToArchive = errorMessageIds.AsSpan().Slice(spanStart, spanLength).ToArray();
            
            await ActAsync(c => c.GetRequiredService<ServiceControlPrdClient>()
                .ArchiveAsync(messagesToArchive, cts));
        }
        
    }

    [TestCase(1, 100, 0)]
    [TestCase(2, 100, 100)]
    [TestCase(72, 100, 7100)]
    public void GetSpanStart(int pageNumber, int pageSize, int expectedSpanStart)
    {
        var spanStart = GetSpanStart(pageNumber, pageSize);
        expectedSpanStart.Should().Be(spanStart);
    }
    
    [TestCase(1, 2, 100, 150, 100)]
    [TestCase(2, 2, 100, 150, 50)]
    [TestCase(72, 72, 100, 7289, 189)]
    public void GetSpanLength(int pageNumber, int numberOfPages, int pageSize, int totalMessages, int expectedSpanLength)
    {
        var spanLength = GetSpanLength(pageNumber, numberOfPages, pageSize, totalMessages);
        expectedSpanLength.Should().Be(spanLength);
    }

    [Test]
    public async Task ClearAllPrdMessages()
    {
        await ArrangeOnDocumentSessionAsync(s =>
        {
            s.DeleteWhere<PrdErrorMessage>(x => true);
            s.DeleteWhere<PrdOneTrackMessage>(x => true);
            return Task.CompletedTask;
        });
    }
    
    [Test]
    public async Task DownloadAllErrorsByExceptionTypeAndStackTraceRecoverabilityGroups()
    {
        var cts = new CancellationTokenSource();
        var recoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlPrdClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.ExceptionTypeAndStackTrace, cts));

        foreach (var recoverabilityGroup in recoverabilityGroups)
        {
            Console.WriteLine($"Downloading {recoverabilityGroup.Count} - {recoverabilityGroup.Title}");
            await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
                .DownloadErrorMessagesForRecoverabilityGroup(recoverabilityGroup.Id,
                    recoverabilityGroup.Count,
                    cts)).ConfigureAwait(false);
        }
    }

    [Test]
    public async Task RetryUowErrors()
    {
        var cts = new CancellationTokenSource();
        var sqlUowErrors = await ArrangeOnDocumentSessionAsync(s => s.Query<PrdErrorMessage>()
            .Where(x => x.Exception.ExceptionType == "System.Data.SqlClient.SqlException")
            .Where(x => x.Exception.StackTrace.Contains("Distributed transaction completed. Either enlist this session in a new transaction or the NULL transaction."))
            .ToListAsync());

        var prdnsbserverTransport = "prdnsbserver.transport";
        //var docautomation = "prdnsbserver.documentautomation";
        var transportMessages = sqlUowErrors
            .Where(x => x.ReceivingEndpoint.Name == prdnsbserverTransport)
            .OrderBy(x => x.TimeOfFailureOffset)
            .ToArray();
        // var daMessages = sqlUowErrors
        //     .Where(x => x.ReceivingEndpoint.Name == docautomation)
        //     .OrderBy(x => x.TimeOfFailureOffset)
        //     .ToArray();

        Console.WriteLine($"transport min {transportMessages.Min(x => x.TimeOfFailureOffset)} max {transportMessages.Max(x => x.TimeOfFailureOffset)}");
        //Console.WriteLine($"docautomation min {daMessages.Min(x => x.TimeOfFailureOffset)} max {daMessages.Max(x => x.TimeOfFailureOffset)}");
        var transportRetryIds = transportMessages.Select(x => x.Id).ToArray();
        Console.WriteLine($"Retrying {transportRetryIds.Length} transport messages");
        await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
             .Retry(transportRetryIds.ToArray(), cts));
        
        // Console.WriteLine("Retrying documentautomation messages");
        // var daRetryIds = daMessages.Select(x => x.Id).ToArray();
        // await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
        //     .Retry(daRetryIds.ToArray(), cts));
    }
    
    [Test]
    public async Task RetryInValidTransactionDescription()
    {

        var cts = new CancellationTokenSource();
        var invalidTransactionDescriptorErrors = await ArrangeOnDocumentSessionAsync(s => s.Query<PrdErrorMessage>()
            .Where(x => x.Exception.ExceptionType == "System.Data.SqlClient.SqlException")
            .Where(x => x.Exception.StackTrace.Contains("System.Data.SqlClient.SqlException (0x80131904)"))
            .ToListAsync());

        var endpoints = invalidTransactionDescriptorErrors.Select(x => x.ReceivingEndpoint).Distinct();
        foreach (var endpoint in endpoints)
        {
            var endpointErrors = invalidTransactionDescriptorErrors
                .Where(x => x.ReceivingEndpoint.Name == endpoint.Name).ToArray();
            Console.WriteLine($"{endpoint.Name} - count - {endpointErrors.Length} min {endpointErrors.Min(x => x.TimeOfFailureOffset)} - max {endpointErrors.Max(x => x.TimeOfFailureOffset)}");
        }
        var transport = "prdnsbserver.transport";
        var transportErrors = invalidTransactionDescriptorErrors
            .Where(x => x.ReceivingEndpoint.Name == transport)
            .OrderBy(x => x.TimeOfFailureOffset)
            .ToArray();
        
        var transportRetryIds = transportErrors.Select(x => x.Id).ToArray();
        Console.WriteLine("Retrying transport messages");
        await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
             .Retry(transportRetryIds.ToArray(), cts));

    }

    [Test]
    public async Task RetryPrimaryFileGroupFullErrors()
    {
        var cts = new CancellationTokenSource();
        var invalidTransactionDescriptorErrors = await ArrangeOnDocumentSessionAsync(s => s.Query<PrdErrorMessage>()
            .Where(x => x.Exception.ExceptionType == "NHibernate.Exceptions.GenericADOException")
            .Where(x => x.Exception.StackTrace.Contains("database 'ONETRACK_VISA_PRD' because the 'PRIMARY' filegroup is full"))
            .ToListAsync());

        var endpoints = invalidTransactionDescriptorErrors.Select(x => x.ReceivingEndpoint).Distinct();
        foreach (var endpoint in endpoints)
        {
            var endpointErrors = invalidTransactionDescriptorErrors
                .Where(x => x.ReceivingEndpoint.Name == endpoint.Name).ToArray();
            Console.WriteLine($"{endpoint.Name} - count - {endpointErrors.Length} min {endpointErrors.Min(x => x.TimeOfFailureOffset)} - max {endpointErrors.Max(x => x.TimeOfFailureOffset)}");
        }

        var thirdAugust = new DateTime(2022, 08, 03);
        var thirdAugustErrors = invalidTransactionDescriptorErrors
            .Where(x => x.TimeOfFailure > thirdAugust)
            .OrderBy(x => x.TimeOfFailureOffset)
            .ToArray();
        
        var retryIds = thirdAugustErrors.Select(x => x.Id).ToArray();
        Console.WriteLine($"Retrying 3 Aug errors {retryIds.Length}");
        Console.WriteLine("Retrying transport messages");
        await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
            .Retry(retryIds.ToArray(), cts));
        
    }
    
    [TestCase("prdnsbserver.fhlinterface")]
    [TestCase("prdnsbserver.emailing")]
    [TestCase("prdnsbserver.transport")]
    public async Task DownloadAllErrorsByEndpointName(string endpointname)
    {
        var cts = new CancellationTokenSource();
        var recoverabilityGroups = await ActAsync(sp => sp
            .GetRequiredService<ServiceControlPrdClient>()
            .GetUnResolvedErrorRecoverabilityGroupsAsync(RecoverabilityGroupType.EndpointName, cts));

        foreach (var recoverabilityGroup in recoverabilityGroups.Where(rg => rg.Title == endpointname))
        {
            Console.WriteLine($"Downloading {recoverabilityGroup.Count} - {recoverabilityGroup.Title}");
            await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
                .DownloadErrorMessagesForRecoverabilityGroup(recoverabilityGroup.Id,
                    recoverabilityGroup.Count,
                    cts)).ConfigureAwait(false);
        }
    }
    
    [Test]
    public async Task RetryRecentKingLivingErrors()
    {
        var cts = new CancellationTokenSource();
        var sqlUowErrors = await ArrangeOnDocumentSessionAsync(s => s.Query<PrdErrorMessage>()
            .Where(x => x.Exception.ExceptionType == "StructureMap.Building.StructureMapBuildException")
            .Where(x => x.Exception.StackTrace.Contains("SqlUnitOfWork.  See the inner exception for details"))
            .ToListAsync());

        var kingliving = "prdnsbserver.kinglivinginterface";
        var docautomation = "prdnsbserver.documentautomation";
        var klMessages = sqlUowErrors
            .Where(x => x.ReceivingEndpoint.Name == kingliving)
            .OrderBy(x => x.TimeOfFailureOffset)
            .ToArray();

        Console.WriteLine($"kingliving count {klMessages.Length} min {klMessages.Min(x => x.TimeOfFailureOffset)} max {klMessages.Max(x => x.TimeOfFailureOffset)}");
        var klRetryIds = klMessages.Select(x => x.Id).ToArray();
        Console.WriteLine("Retrying kingliving messages");
        await ActAsync(sp => sp.GetRequiredService<ServiceControlPrdDownloadService>()
            .Retry(klRetryIds.ToArray(), cts));
        
    }
    
    [TestCase("prdnsbserver.emailing")]
    public async Task Available_MessageTypes_ByEndpointName(string endpoint)
    {
        var cts = new CancellationTokenSource();
        var messageTypes = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messageTypes = await s.Query<PrdErrorMessage>()
                .Where(x => x.ReceivingEndpoint.Name.Equals(endpoint, StringComparison.OrdinalIgnoreCase))
                .ToListAsync(cts.Token);

            return messageTypes.GroupBy(x => x.MessageType)
                .Select(grp => new MessageCountByType(grp.Key, grp.Count(), grp.Min(x => x.TimeOfFailure), grp.Max(x => x.TimeOfFailure)))
                .ToList();
        });
        messageTypes.ForEach(Console.WriteLine);
    }

    private record MessageCountByType(string MessageType, int Count, DateTime Eearliest, DateTime Latest);
    
    [TestCase("Visa.Zaphod.FhlInterface.Commands.InitiateTransportOnlyPoRequestProcessingCommand")]
    [TestCase("Visa.Zaphod.Emailing.Messages.Commands.SendEmailCommand")]
    public async Task Download_MessageType(string messageType)
    {
        var cts = new CancellationTokenSource();
        var messageIds = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messageIds = await s.Query<PrdErrorMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.MessageId)
                .ToListAsync(cts.Token);
            return messageIds;
        });

        await ActAsync(c => c.GetRequiredService<ServiceControlPrdDownloadService>()
            .DownloadMessageBlobsAsync<PrdOneTrackMessage>(messageType, messageIds.ToArray(), cts));

        await AssertOnDocumentSessionAsync(async s =>
        {
            var persistedCount = await s.Query<PrdOneTrackMessage>()
                .CountAsync(x => x.MessageType == messageType);

            persistedCount.Should().Be(messageIds.Count);
        });
    }
    
    [Test]
    public async Task Analyse_InitiateTransportOnlyPoRequestProcessingCommand()
    {
        var messageType = "Visa.Zaphod.FhlInterface.Commands.InitiateTransportOnlyPoRequestProcessingCommand";
        var messagesDataset = await GetMessagesDataset(messageType);

        var cmds = messagesDataset.Tables["DeliveryJobDataFromTransportOnlyPurchaseOrderLine"];
        cmds.AsEnumerable()
            .Select(x => new {
                CustomerPo = x.Field<string>("CustomerPo"),
                ShipId = x.Field<string>("ShipId"),
                ContainerNumber = x.Field<string>("ContainerNumber"),
                //FhlResponseReceivedTimestamp = x.Field<string>("FhlResponseReceivedTimestamp")
            })
            .Distinct()
            .ToList()
            .ForEach(Console.WriteLine);

        
    }
    
    [Test]
    public async Task Analyse_SendEmailCommand()
    {
        var messageType = "Visa.Zaphod.Emailing.Messages.Commands.SendEmailCommand";

        var messages = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messages = await s.Query<PrdOneTrackMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
            return messages;
        });

        var failedMailAddresses = messages
            .Select(m => new
            {
                id = m.Id,
                xDoc = XDocument.Parse(m.MessageBlob)
            })
            .SelectMany(o =>
            {
                var nsMgr = new XmlNamespaceManager(new NameTable());
                nsMgr.AddNamespace("ex", o.xDoc.Root.Name.NamespaceName);
                var addresses = o.xDoc
                    .XPathSelectElements("/ex:SendEmailCommand/ex:EmailProperties/ex:To/ex:MailAddress/ex:Address",
                        nsMgr)
                    .AsEnumerable()
                    .Select(x => x.Value)
                    .ToArray();
                var subject = o.xDoc.XPathSelectElement("/ex:SendEmailCommand/ex:EmailProperties/ex:Subject", nsMgr)
                    ?.Value;
                return addresses.Select(a => new FailedMailAddress(a, subject, o.id)).ToArray();
            })
            .ToArray();
        var messageIds = failedMailAddresses.Select(x => x.MessageId).ToArray();
        
        var allErrors = await ArrangeOnDocumentSessionAsync(async s => await s.Query<PrdErrorMessage>().ToListAsync());

        var errors = allErrors.Where(x => messageIds.Contains(x.MessageId)).ToArray();

        failedMailAddresses = failedMailAddresses
            .Select(f =>
            {
                var error = errors.FirstOrDefault(x => x.MessageId == f.MessageId);
                return f with { ErrorMessage = error };
            })
            .ToArray();
        var errorsToArchive = failedMailAddresses
            .Where(x => x.ErrorMessage.Exception.StackTrace.Contains("The remote name could not be resolved: 'visaglobal-com-au.mail.protection.outlook.com'"))
            .Select(x => x.ErrorMessage!.Id)
            .ToArray();

        var nonArchiveErrors = failedMailAddresses
            .Where(x => !errorsToArchive.Contains(x.ErrorMessage.Id))
            .ToArray();
        
        var accessDenied = nonArchiveErrors.Where(x =>
                x.ErrorMessage.Exception.Message.Contains(
                    "Mailbox unavailable. The server response was: 5.4.1 Recipient address rejected: Access denied. AS(201806281)"))
            .ToArray();
        
        Console.WriteLine($"{nonArchiveErrors.Length} - {accessDenied.Length}");
        foreach(var grp in accessDenied.GroupBy(x => x.MailAddress))
        {
            Console.WriteLine($"{grp.Key}, Count {grp.Count()} Earliest {grp.Min(x => x.ErrorMessage.TimeOfFailure)}, Latest {grp.Max(x => x.ErrorMessage.TimeOfFailure)}");
        }

         Console.WriteLine($"To archive - {errorsToArchive.Length}");
        // var cts = new CancellationTokenSource();
        // await ActAsync(c => c.GetRequiredService<ServiceControlPrdClient>()
        //     .ArchiveAsync(errorsToArchive, cts));
    }

    private static async Task<DataSet> GetMessagesDataset(string messageType)
    {
        var messagesDataset = await ArrangeOnDocumentSessionAsync(async s =>
        {
            var messages = await s.Query<PrdOneTrackMessage>()
                .Where(x => x.MessageType.Equals(messageType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            var messagesDatasets = new List<DataSet>();
            foreach (var message in messages)
            {
                var ds = new DataSet();
                using var sr = new StringReader(message.MessageBlob);
                ds.ReadXml(sr);
                messagesDatasets.Add(ds);
            }

            var messagesDataset = new DataSet();
            foreach (var messageDataSet in messagesDatasets)
            {
                messagesDataset.Merge(messageDataSet);
            }

            return messagesDataset;
        });
        return messagesDataset;
    }

    private static int GetSpanLength(int pageNumber, int numberOfPages, int pageSize, int totalMessages)
    {
        return pageNumber != numberOfPages
            ? pageSize
            : totalMessages - GetSpanStart(pageNumber, pageSize);
    }

    private static int GetSpanStart(int pageNumber, int pageSize)
    {
        return (pageNumber - 1) * pageSize;
    }

    public record PurchaseOrderAssignedToShipmentNsbEvent
    {
        public string PurchaseOrderId { get; init; }
        public string ShipmentId { get; init; }
        public string CustomerId { get; init; }
        public string CustomerPo { get; init; }
    }

    public record FailedMailAddress(string MailAddress, string Subject, string MessageId, PrdErrorMessage? ErrorMessage = null);
}