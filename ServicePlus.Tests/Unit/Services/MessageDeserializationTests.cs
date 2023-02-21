using System;
using System.Globalization;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using ServicePlus.Core.Entities;

namespace ServicePlus.Tests.Unit.Services;

[TestFixture]
public class MessageDeserializationTests
{
    [TestCase("TestData/Messages/MessagesSearch.json")]
    public void Deserializes(string fileName)
    {
        var filePath = $"{TestContext.CurrentContext.TestDirectory}/{fileName}";
        var jsonText = File.ReadAllText(filePath);

        var messages = JsonConvert.DeserializeObject<Message[]>(jsonText);
        messages.Should().NotBeNull();
        messages!.Length.Should().Be(3);

        messages[0]
            .Should()
            .BeEquivalentTo(new Message
            {
                Id = "7ab695f0-f206-6ad0-5c7b-31e2947e2e48",
                MessageId = "6f43e93a-eaae-438c-b402-ae9200ced473",
                MessageType = "NsbMessages.ShippingDomain.Events.DeliveryJobVoyageUpdateEvent",
                SendingEndpoint = new Endpoint
                {
                    Name = "qasnsbserver",
                    HostId = "e938fc13-0990-dd39-8f87-f438964a6515",
                    Host = "VGL1ONETRTEST"
                },
                ReceivingEndpoint = new Endpoint
                {
                    Name = "qasnsbserver.shippingdomain",
                    HostId = "ab205d7f-76bd-a515-fdfd-59e309e2d646",
                    Host = "VGL1ONETRTEST"
                },
                TimeSent = DateTime.Parse("2022-05-11T02:34:05.38505Z", null, DateTimeStyles.RoundtripKind),
                ProcessesAt = DateTime.Parse("2022-05-11T02:34:05.511037Z", null, DateTimeStyles.RoundtripKind),
                CriticalTime = TimeSpan.Parse("00:00:00"),
                ProcessingTime = TimeSpan.Parse("00:00:00"),
                DeliveryTime = TimeSpan.Parse("00:00:00"),
                IsSystemMessage = false,
                ConversationId = "6f89fcf3-eec1-49ed-a7c7-ae9200ced3d4",
                Headers = new HeaderKeyValue[]
                {
                    new()
                    {
                        Key = "NServiceBus.MessageId",
                        Value = "6f43e93a-eaae-438c-b402-ae9200ced473"
                    },
                    new()
                    {
                        Key = "NServiceBus.CorrelationId",
                        Value = "77a92794-6a48-445b-ac8a-ae9200ced476"
                    },
                    new()
                    {
                        Key = "NServiceBus.MessageIntent",
                        Value = "Publish"
                    },
                    new()
                    {
                        Key = "NServiceBus.Version",
                        Value = "5.2.26"
                    },
                    new()
                    {
                        Key = "NServiceBus.TimeSent",
                        Value = "2022-05-11 02:34:05:385050 Z"
                    },
                    new()
                    {
                        Key = "SignalRConnectionId",
                        Value = default
                    },
                    new()
                    {
                        Key = "UserId",
                        Value = "3866"
                    },
                    new()
                    {
                        Key = "UserRef",
                        Value =
                            "{\"id\":3866,\"userId\":\"gsingh\",\"fullName\":\"Gursharan Singh\",\"emailAddress\":\"gsingh@visaglobal.com.au\"}"
                    },
                    new()
                    {
                        Key = "CommandSentUtc",
                        Value = "\"2022-05-11T02:32:59.227Z\""
                    },
                    new()
                    {
                        Key = "NServiceBus.ContentType",
                        Value = "text/xml"
                    },
                    new()
                    {
                        Key = "NServiceBus.EnclosedMessageTypes",
                        Value =
                            "NsbMessages.ShippingDomain.Events.DeliveryJobVoyageUpdateEvent, NsbMessages, Version=2022.10.4895.0, Culture=neutral, PublicKeyToken=null;NsbMessages.ShippingDomain.Events.IDeliveryJobVoyageUpdatedEvent, NsbMessages, Version=2022.10.4895.0, Culture=neutral, PublicKeyToken=null"
                    },
                    new()
                    {
                        Key = "WinIdName",
                        Value = "VISAAUST\\ONETRACK_VISA_QAS"
                    },
                    new()
                    {
                        Key = "NServiceBus.RelatedTo",
                        Value = "e72e4dc5-c925-45d9-822a-ae9200ced3ef"
                    },
                    new()
                    {
                        Key = "NServiceBus.ConversationId",
                        Value = "6f89fcf3-eec1-49ed-a7c7-ae9200ced3d4"
                    },
                    new()
                    {
                        Key = "CorrId",
                        Value = "6f43e93a-eaae-438c-b402-ae9200ced473\\0"
                    },
                    new()
                    {
                        Key = "NServiceBus.OriginatingMachine",
                        Value = "VGL1ONETRTEST"
                    },
                    new()
                    {
                        Key = "NServiceBus.OriginatingEndpoint",
                        Value = "qasnsbserver"
                    },
                    new()
                    {
                        Key = "$.diagnostics.originating.hostid",
                        Value = "e938fc130990dd398f87f438964a6515"
                    },
                    new()
                    {
                        Key = "NServiceBus.ReplyToAddress",
                        Value = "qasnsbserver@VGL1ONETRTEST"
                    },
                    new()
                    {
                        Key = "$.diagnostics.hostid",
                        Value = "ab205d7f76bda515fdfd59e309e2d646"
                    },
                    new()
                    {
                        Key = "$.diagnostics.hostdisplayname",
                        Value = "VGL1ONETRTEST"
                    },
                    new()
                    {
                        Key = "NServiceBus.ExceptionInfo.ExceptionType",
                        Value = "System.InvalidOperationException"
                    },
                    new()
                    {
                        Key = "NServiceBus.ExceptionInfo.HelpLink",
                        Value = null
                    },
                    new()
                    {
                        Key = "NServiceBus.ExceptionInfo.Message",
                        Value =
                            "No handlers could be found for message type: NsbMessages.ShippingDomain.Events.DeliveryJobVoyageUpdateEvent"
                    },
                    new()
                    {
                        Key = "NServiceBus.ExceptionInfo.Source",
                        Value = "NServiceBus.Core"
                    },
                    new()
                    {
                        Key = "NServiceBus.ExceptionInfo.StackTrace",
                        Value =
                            "System.InvalidOperationException: No handlers could be found for message type: NsbMessages.ShippingDomain.Events.DeliveryJobVoyageUpdateEvent\n   at NServiceBus.LoadHandlersBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at Visa.Zaphod.ShippingDomain.Configuration.Uow.UnitOfWorkBehavior.Invoke(IncomingContext context, Action next) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\NsbServer.ShippingDomain\\Configuration\\Uow\\UnitOfWork.cs:line 44\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at Visa.Zaphod.ShippingDomain.Configuration.Dtc.DtcErrorTrackingBehaviour.Invoke(IncomingContext context, Action next) in D:\\BuildAgent\\work\\b2aa8b18f65acc35\\NsbServer.ShippingDomain\\Configuration\\Dtc\\DtcErrorTrackingBehaviour.cs:line 27\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ExecuteLogicalMessagesBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.UnitOfWorkBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ChildContainerBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.ProcessingStatisticsBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.AuditBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.<>c__DisplayClass4_1.<InvokeNext>b__0()\n   at NServiceBus.Metrics.ServiceControl.ServiceControlMonitoring.ServiceControlMonitoringRegistrationBehavior.Invoke(IncomingContext context, Action next)\n   at NServiceBus.BehaviorChain`1.InvokeNext(T context)\n   at NServiceBus.BehaviorChain`1.Invoke()\n   at NServiceBus.Pipeline.PipelineExecutor.Execute[T](BehaviorChain`1 pipelineAction, T context)\n   at NServiceBus.Unicast.Transport.TransportReceiver.OnTransportMessageReceived(TransportMessage msg)\n   at NServiceBus.Unicast.Transport.TransportReceiver.ProcessMessage(TransportMessage message)\n   at NServiceBus.Unicast.Transport.TransportReceiver.TryProcess(TransportMessage message)\n   at NServiceBus.Transports.Msmq.MsmqDequeueStrategy.Action(TransactionSettings transactionSettings, TransactionOptions transactionOptions, MsmqUnitOfWork unitOfWork, MessageQueue receiveQueue, MessageQueue errorQueue, CircuitBreaker circuitBreaker, CriticalError criticalError, AutoResetEvent peekResetEvent, TimeSpan receiveTimeout, SemaphoreSlim throttlingSemaphore, Func`2 tryProcessMessage, Action`2 endProcessMessage)"
                    },
                    new()
                    {
                        Key = "NServiceBus.FailedQ",
                        Value = "qasnsbserver.shippingdomain@VGL1ONETRTEST"
                    },
                    new()
                    {
                        Key = "NServiceBus.TimeOfFailure",
                        Value = "2022-05-11 02:34:05:511037 Z"
                    },
                    new()
                    {
                        Key = "NServiceBus.Retries.Timestamp",
                        Value = "2022-05-11 02:33:02:856365 Z"
                    },
                    new()
                    {
                        Key = "NServiceBus.Timeout.RouteExpiredTimeoutTo",
                        Value = "qasnsbserver.shippingdomain@VGL1ONETRTEST"
                    },
                    new()
                    {
                        Key = "NServiceBus.Timeout.Expire",
                        Value = "2022-05-11 02:34:04:669988 Z"
                    },
                    new()
                    {
                        Key = "NServiceBus.RelatedToTimeoutId",
                        Value = null
                    }
                },
                Status = "failed",
                MessageIntent = "publish",
                BodyUrl =
                    "/messages/6f43e93a-eaae-438c-b402-ae9200ced473/body?instance_id=aHR0cDovL2xvY2FsaG9zdDozMzMzMy9hcGk.",
                BodySize = 477,
                InstanceId = "aHR0cDovL2xvY2FsaG9zdDozMzMzMy9hcGk."
            });
    }
}