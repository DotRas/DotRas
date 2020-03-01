using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Infrastructure.Advice;
using DotRas.Internal.Interop;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Infrastructure.Advice
{
    [TestFixture]
    public class RasApi32LoggingAdviceTests
    {
        private delegate void LogEventCallback(EventLevel eventLevel, TraceEvent eventData);
        
        private Mock<IRasApi32> api;
        private Mock<IEventLoggingPolicy> eventLoggingPolicy;

        [SetUp]
        public void Setup()
        {
            api = new Mock<IRasApi32>();
            eventLoggingPolicy = new Mock<IEventLoggingPolicy>();
        }

        [Test]
        public void RasClearConnectionStatisticsAsExpected()
        {
            var hRasConn = new IntPtr(1);

            api.Setup(o => o.RasClearConnectionStatistics(hRasConn)).Returns(SUCCESS);
            eventLoggingPolicy.Setup(o => o.LogEvent(It.IsAny<EventLevel>(), It.IsAny<PInvokeInt32CallCompletedTraceEvent>())).Callback(new LogEventCallback(
                (level, o1) =>
                {
                    Assert.AreEqual(EventLevel.Verbose, level);

                    var eventData = (PInvokeInt32CallCompletedTraceEvent)o1;
                    Assert.AreEqual(hRasConn, (IntPtr)eventData.Args[nameof(hRasConn)]);
                    Assert.True(eventData.Duration > TimeSpan.Zero);
                    Assert.AreEqual(SUCCESS, eventData.Result);
                })).Verifiable();

            var target = new RasApi32LoggingAdvice(api.Object, eventLoggingPolicy.Object);
            var result = target.RasClearConnectionStatistics(hRasConn);

            eventLoggingPolicy.Verify();
            Assert.AreEqual(SUCCESS, result);
        }

        [Test]
        public void RasConnectionNotificationAsExpected()
        {
            var hRasConn = new IntPtr(1);
            var hEvent = new Mock<ISafeHandleWrapper>();
            var dwFlags = RASCN.Connection;

            api.Setup(o => o.RasConnectionNotification(hRasConn, hEvent.Object, dwFlags)).Returns(SUCCESS);
            eventLoggingPolicy.Setup(o => o.LogEvent(It.IsAny<EventLevel>(), It.IsAny<PInvokeInt32CallCompletedTraceEvent>())).Callback(new LogEventCallback(
                (level, o1) =>
                {
                    Assert.AreEqual(EventLevel.Verbose, level);

                    var eventData = (PInvokeInt32CallCompletedTraceEvent)o1;
                    Assert.AreEqual(hRasConn, (IntPtr)eventData.Args[nameof(hRasConn)]);
                    Assert.AreEqual(hEvent.Object, (ISafeHandleWrapper)eventData.Args[nameof(hEvent)]);
                    Assert.AreEqual(dwFlags, (RASCN)eventData.Args[nameof(dwFlags)]);
                    Assert.True(eventData.Duration > TimeSpan.Zero);
                    Assert.AreEqual(SUCCESS, eventData.Result);
                })).Verifiable();

            var target = new RasApi32LoggingAdvice(api.Object, eventLoggingPolicy.Object);
            var result = target.RasConnectionNotification(hRasConn, hEvent.Object, dwFlags);

            eventLoggingPolicy.Verify();
            Assert.AreEqual(SUCCESS, result);
        }

        private delegate int RasEnumConnectionsCallback(RASCONN[] lpRasConn, ref int lpCb, ref int lpConnections);

        [Test]
        public void RasEnumConnectionsAsExpected()
        {
            var lpCb = 0;
            var lpConnections = 1;
            var lpRasConn = new RASCONN[0];

            api.Setup(o => o.RasEnumConnections(lpRasConn, ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumConnectionsCallback(
                (RASCONN[] o1, ref int o2, ref int o3) =>
                {
                    o2 = 1;
                    o3 = 2;
                    return SUCCESS;
                }));

            eventLoggingPolicy.Setup(o => o.LogEvent(It.IsAny<EventLevel>(), It.IsAny<PInvokeInt32CallCompletedTraceEvent>())).Callback(new LogEventCallback(
                (level, o1) =>
                {
                    Assert.AreEqual(EventLevel.Verbose, level);

                    var eventData = (PInvokeInt32CallCompletedTraceEvent)o1;
                    Assert.True(eventData.Args.ContainsKey(nameof(lpRasConn)));
                    Assert.AreEqual(0, (int)eventData.Args[nameof(lpCb)]);
                    Assert.AreEqual(1, (int)eventData.Args[nameof(lpConnections)]);
                    Assert.AreEqual(1, (int)eventData.OutArgs[nameof(lpCb)]);
                    Assert.AreEqual(2, (int)eventData.OutArgs[nameof(lpConnections)]);
                    Assert.True(eventData.Duration > TimeSpan.Zero);
                    Assert.AreEqual(SUCCESS, eventData.Result);
                })).Verifiable();

            var target = new RasApi32LoggingAdvice(api.Object, eventLoggingPolicy.Object);
            var result = target.RasEnumConnections(lpRasConn, ref lpCb, ref lpConnections);

            eventLoggingPolicy.Verify();
            Assert.AreEqual(SUCCESS, result);
        }

        private delegate int RasEnumDevicesCallback(RASDEVINFO[] lpRasDevInfo, ref int lpCb, ref int lpcDevices);

        [Test]
        public void RasEnumDevicesAsExpected()
        {
            var lpCb = 0;
            var lpcDevices = 1;
            var lpRasDevInfo = new RASDEVINFO[0];

            api.Setup(o => o.RasEnumDevices(lpRasDevInfo, ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(new RasEnumDevicesCallback(
                (RASDEVINFO[] o1, ref int o2, ref int o3) =>
                {
                    o2 = 1;
                    o3 = 2;

                    return SUCCESS;
                }));

            eventLoggingPolicy.Setup(o => o.LogEvent(It.IsAny<EventLevel>(), It.IsAny<PInvokeInt32CallCompletedTraceEvent>())).Callback(new LogEventCallback(
                (level, o1) =>
                {
                    Assert.AreEqual(EventLevel.Verbose, level);

                    var eventData = (PInvokeInt32CallCompletedTraceEvent)o1;
                    Assert.True(eventData.Args.ContainsKey(nameof(lpRasDevInfo)));
                    Assert.AreEqual(0, (int)eventData.Args[nameof(lpCb)]);
                    Assert.AreEqual(1, (int)eventData.Args[nameof(lpcDevices)]);
                    Assert.AreEqual(1, (int)eventData.OutArgs[nameof(lpCb)]);
                    Assert.AreEqual(2, (int)eventData.OutArgs[nameof(lpcDevices)]);
                    Assert.True(eventData.Duration > TimeSpan.Zero);
                    Assert.AreEqual(SUCCESS, eventData.Result);
                })).Verifiable();

            var target = new RasApi32LoggingAdvice(api.Object, eventLoggingPolicy.Object);
            var result = target.RasEnumDevices(lpRasDevInfo, ref lpCb, ref lpcDevices);

            eventLoggingPolicy.Verify();
            Assert.AreEqual(SUCCESS, result);
        }

        private delegate int RasDialCallback(ref RASDIALEXTENSIONS lpRasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS lpRasDialParams, NotifierType dwNotifierType, RasDialFunc2 lpvNotifier, out IntPtr lphRasConn);

        [Test]
        public void RasDialAsExpected()
        {
            var lpRasDialExtensions = new RASDIALEXTENSIONS();
            var lpszPhoneBook = @"C:\Users\My.pbk";

            var lpRasDialParams = new RASDIALPARAMS();
            var dwNotifierType = NotifierType.RasDialFunc2;
            RasDialFunc2 lpvNotifier = null;
            var lphRasConn = IntPtr.Zero;

            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, lpszPhoneBook, ref It.Ref<RASDIALPARAMS>.IsAny, dwNotifierType, lpvNotifier, out It.Ref<IntPtr>.IsAny)).Returns(new RasDialCallback(
                (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, NotifierType o4, RasDialFunc2 o5, out IntPtr o6) =>
                {
                    o6 = new IntPtr(1);
                    return SUCCESS;
                }));

                eventLoggingPolicy.Setup(o => o.LogEvent(It.IsAny<EventLevel>(), It.IsAny<PInvokeInt32CallCompletedTraceEvent>())).Callback(new LogEventCallback(
                (level, o1) =>
                {
                    Assert.AreEqual(EventLevel.Verbose, level);

                    var eventData = (PInvokeInt32CallCompletedTraceEvent)o1;
                    Assert.True(eventData.Args.ContainsKey(nameof(lpRasDialExtensions)));
                    Assert.True(eventData.Args.ContainsKey(nameof(lpszPhoneBook)));
                    Assert.True(eventData.Args.ContainsKey(nameof(lpRasDialParams)));
                    Assert.True(eventData.Args.ContainsKey(nameof(dwNotifierType)));
                    Assert.True(eventData.Args.ContainsKey(nameof(lpvNotifier)));
                    Assert.True(eventData.OutArgs.ContainsKey(nameof(lphRasConn)));

                    Assert.True(eventData.Duration > TimeSpan.Zero);
                    Assert.AreEqual(SUCCESS, eventData.Result);
                })).Verifiable();

            var target = new RasApi32LoggingAdvice(api.Object, eventLoggingPolicy.Object);
            var result = target.RasDial(ref lpRasDialExtensions, lpszPhoneBook, ref lpRasDialParams, dwNotifierType, null, out lphRasConn);

            eventLoggingPolicy.Verify();
            Assert.AreEqual(SUCCESS, result);
            Assert.AreEqual(new IntPtr(1), lphRasConn);
        }
    }
}