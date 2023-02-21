using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using FluentAssertions;
using NUnit.Framework;
using ServicePlus.Core.Entities;
using Exception = ServicePlus.Core.Entities.Exception;

namespace ServicePlus.Tests.Unit.Services;

[TestFixture]
public class ErrorMessageDeserializationTests
{
    
    [TestCase("TestData/ErrorMessages/TwoErrorMessages.json")]
    public void Deserializes(string fileName)
    {
        var filePath = $"{TestContext.CurrentContext.TestDirectory}/{fileName}";
        var jsonText = File.ReadAllText(filePath);

        var errorMessages = JsonConvert.DeserializeObject<ErrorMessage[]>(jsonText);

        errorMessages.Should().NotBeNull();
        errorMessages!.Length.Should().Be(2);
        errorMessages
            .Should()
            .BeEquivalentTo(new[]
            {
                new ErrorMessage
                {
                    Id = "9ef45061-2e89-3a40-0f89-81b92ea5bb70",
                    MessageType = "Visa.Zaphod.ShippingDomain.Commands.UploadDocs.SetUploadDocParentCommand",
                    TimeSent = DateTime.Parse("2022-05-11T07:26:31.011608Z", null, DateTimeStyles.RoundtripKind),
                    IsSystemMessage = false,
                    Exception = new Exception
                    {
                        ExceptionType = "Visa.Zaphod.Core.Exceptions.OneTrackException",
                        Message = "Invalid doc: HBL 1280031",
                        Source = "OneTrackClasses",
                        StackTrace =
                            "Visa.Zaphod.Core.Exceptions.OneTrackException: Invalid doc: HBL 1280031\n   at VisaOneTrack.Extensions.DocKeyExtensions.GetOneTrackDocument(ISession session, DocType docType, Int32 id)\n   at Visa.Zaphod.QueryServices.UploadDocQueryService.GetUploadParent(DocKey docKey) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\Zaphod\\QueryServices\\Visa.Zaphod.QueryServices\\UploadDocQueryService.cs:line 411\n   at Visa.Zaphod.QueryServices.UploadDocQueryService.GetShipmentIdsForParentDocKey(DocKey docKey) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\Zaphod\\QueryServices\\Visa.Zaphod.QueryServices\\UploadDocQueryService.cs:line 421\n   at Visa.Zaphod.ViewModels.Persisted.EventHandlers.ShipmentStepStatusUpdates.UpdateShipmentStepStatusWhenUploadDocParentChanges.Handle(UploadDocParentChangedEvent t) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\Zaphod\\ViewModels\\Visa.Zaphod.ViewModels.Grids\\EventHandlers\\ShipmentStepStatusUpdates\\UpdateShipmentStepStatusWhenUploadDocParentChanges.cs:line 31\n   at Visa.Zaphod.Core.Nsb5.EventProcessor.Raise[T](T domainEvent)\n   at VisaOneTrack.UploadDoc.SetParentDoc(DocKey newDocKey)\n   at NServiceBus.Unicast.MessageHandlerRegistry.Invoke(Object handler, Object message, Dictionary`2 dictionary)\n   at NServiceBus.InvokeHandlersBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.SetCurrentMessageBeingHandledBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.LoadHandlersBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at Visa.Zaphod.ShippingDomain.Configuration.Uow.UnitOfWorkBehavior.Invoke(IncomingContext context, Action next) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\NsbServer.ShippingDomain\\Configuration\\Uow\\UnitOfWork.cs:line 44\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at Visa.Zaphod.ShippingDomain.Configuration.Dtc.DtcErrorTrackingBehaviour.Invoke(IncomingContext context, Action next) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\NsbServer.ShippingDomain\\Configuration\\Dtc\\DtcErrorTrackingBehaviour.cs:line 27\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ExecuteLogicalMessagesBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.UnitOfWorkBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ChildContainerBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ProcessingStatisticsBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.AuditBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.Metrics.ServiceControl.ServiceControlMonitoring.ServiceControlMonitoringRegistrationBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.Invoke()\n   at NServiceBus.Pipeline.PipelineExecutor.Execute[T](BehaviorChain`1 pipelineAction, T context)\n   at NServiceBus.Unicast.Transport.TransportReceiver.OnTransportMessageReceived(TransportMessage msg)\n   at NServiceBus.Unicast.Transport.TransportReceiver.ProcessMessage(TransportMessage message)\n   at NServiceBus.Unicast.Transport.TransportReceiver.TryProcess(TransportMessage message)\n   at NServiceBus.Transports.Msmq.MsmqDequeueStrategy.Action(TransactionSettings transactionSettings, TransactionOptions transactionOptions, MsmqUnitOfWork unitOfWork, MessageQueue receiveQueue, MessageQueue errorQueue, CircuitBreaker circuitBreaker, CriticalError criticalError, AutoResetEvent peekResetEvent, TimeSpan receiveTimeout, SemaphoreSlim throttlingSemaphore, Func`2 tryProcessMessage, Action`2 endProcessMessage)"
                    },
                    MessageId = "bc88e107-f1e5-45e6-8ac1-ae92011f26cc",
                    NumberOfProcessingAttempts = 1,
                    Status = "unresolved",
                    SendingEndpoint = new Endpoint
                    {
                        Name = "prd.visaonetrack.web.vgl1onetrweb1",
                        HostId = "c7c8be79-4f63-90df-a09c-f3c87075653f",
                        Host = "VGL1ONETRWEB1"
                    },
                    ReceivingEndpoint = new Endpoint
                    {
                        Name = "prdnsbserver.shippingdomain",
                        HostId = "bfb4ddda-07eb-13ca-5f27-cb279e4507df",
                        Host = "VGL1ONETRSBS"
                    },
                    QueueAddress = "prdnsbserver.shippingdomain@VGL1ONETRSBS",
                    TimeOfFailure = DateTime.Parse("2022-05-11T07:26:31.167866Z", null, DateTimeStyles.RoundtripKind),
                    LastModified = DateTime.Parse("2022-05-11T07:26:31.2044525Z", null, DateTimeStyles.RoundtripKind),
                    Edited = false,
                    EditOf = ""
                },
                new ErrorMessage
                {
                    Id = "d725a02d-0291-bc5b-356e-92b5b0eba6e3",
                    MessageType = "Visa.Zaphod.ShippingDomain.Commands.UploadDocs.SetUploadDocParentCommand",
                    TimeSent = DateTime.Parse("2022-05-11T07:26:24.870735Z", null, DateTimeStyles.RoundtripKind),
                    IsSystemMessage = false,
                    Exception = new Exception
                    {
                        ExceptionType = "Visa.Zaphod.Core.Exceptions.OneTrackException",
                        Message = "Invalid doc: HBL 1280031",
                        Source = "OneTrackClasses",
                        StackTrace =
                            "Visa.Zaphod.Core.Exceptions.OneTrackException: Invalid doc: HBL 1280031\n   at VisaOneTrack.Extensions.DocKeyExtensions.GetOneTrackDocument(ISession session, DocType docType, Int32 id)\n   at Visa.Zaphod.QueryServices.UploadDocQueryService.GetUploadParent(DocKey docKey) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\Zaphod\\QueryServices\\Visa.Zaphod.QueryServices\\UploadDocQueryService.cs:line 411\n   at Visa.Zaphod.QueryServices.UploadDocQueryService.GetShipmentIdsForParentDocKey(DocKey docKey) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\Zaphod\\QueryServices\\Visa.Zaphod.QueryServices\\UploadDocQueryService.cs:line 421\n   at Visa.Zaphod.ViewModels.Persisted.EventHandlers.ShipmentStepStatusUpdates.UpdateShipmentStepStatusWhenUploadDocParentChanges.Handle(UploadDocParentChangedEvent t) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\Zaphod\\ViewModels\\Visa.Zaphod.ViewModels.Grids\\EventHandlers\\ShipmentStepStatusUpdates\\UpdateShipmentStepStatusWhenUploadDocParentChanges.cs:line 31\n   at Visa.Zaphod.Core.Nsb5.EventProcessor.Raise[T](T domainEvent)\n   at VisaOneTrack.UploadDoc.SetParentDoc(DocKey newDocKey)\n   at NServiceBus.Unicast.MessageHandlerRegistry.Invoke(Object handler, Object message, Dictionary`2 dictionary)\n   at NServiceBus.InvokeHandlersBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.SetCurrentMessageBeingHandledBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.LoadHandlersBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at Visa.Zaphod.ShippingDomain.Configuration.Uow.UnitOfWorkBehavior.Invoke(IncomingContext context, Action next) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\NsbServer.ShippingDomain\\Configuration\\Uow\\UnitOfWork.cs:line 44\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at Visa.Zaphod.ShippingDomain.Configuration.Dtc.DtcErrorTrackingBehaviour.Invoke(IncomingContext context, Action next) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\NsbServer.ShippingDomain\\Configuration\\Dtc\\DtcErrorTrackingBehaviour.cs:line 27\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ExecuteLogicalMessagesBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.UnitOfWorkBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ChildContainerBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ProcessingStatisticsBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.AuditBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.Metrics.ServiceControl.ServiceControlMonitoring.ServiceControlMonitoringRegistrationBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.Invoke()\n   at NServiceBus.Pipeline.PipelineExecutor.Execute[T](BehaviorChain`1 pipelineAction, T context)\n   at NServiceBus.Unicast.Transport.TransportReceiver.OnTransportMessageReceived(TransportMessage msg)\n   at NServiceBus.Unicast.Transport.TransportReceiver.ProcessMessage(TransportMessage message)\n   at NServiceBus.Unicast.Transport.TransportReceiver.TryProcess(TransportMessage message)\n   at NServiceBus.Transports.Msmq.MsmqDequeueStrategy.Action(TransactionSettings transactionSettings, TransactionOptions transactionOptions, MsmqUnitOfWork unitOfWork, MessageQueue receiveQueue, MessageQueue errorQueue, CircuitBreaker circuitBreaker, CriticalError criticalError, AutoResetEvent peekResetEvent, TimeSpan receiveTimeout, SemaphoreSlim throttlingSemaphore, Func`2 tryProcessMessage, Action`2 endProcessMessage)"
                    },
                    MessageId = "c278bf73-067e-469a-9c18-ae92011f1eaf",
                    NumberOfProcessingAttempts = 1,
                    Status = "unresolved",
                    SendingEndpoint = new Endpoint
                    {
                        Name = "prd.visaonetrack.web.vgl1onetrweb1",
                        HostId = "c7c8be79-4f63-90df-a09c-f3c87075653f",
                        Host = "VGL1ONETRWEB1"
                    },
                    ReceivingEndpoint = new Endpoint
                    {
                        Name = "prdnsbserver.shippingdomain",
                        HostId = "bfb4ddda-07eb-13ca-5f27-cb279e4507df",
                        Host = "VGL1ONETRSBS"
                    },
                    QueueAddress = "prdnsbserver.shippingdomain@VGL1ONETRSBS",
                    TimeOfFailure = DateTime.Parse("2022-05-11T07:26:25.073872Z", null, DateTimeStyles.RoundtripKind),
                    LastModified = DateTime.Parse("2022-05-11T07:26:25.1105926Z", null, DateTimeStyles.RoundtripKind),
                    Edited = false,
                    EditOf = ""
                }
            });
    }
}