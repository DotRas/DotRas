using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Diagnostics
{
    [TestFixture]
    public class DefaultEventLoggingPolicyTests
    {
        [Test]
        public void ThrowsAnExceptionWhenTheLogIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new DefaultEventLoggingPolicy(null);
            });
        }

        [Test]
        public void ThrowsAnExceptionWhenTheEventDataIsNull()
        {
            var target = new DefaultEventLoggingPolicy(new Mock<ILog>().Object);
            Assert.Throws<ArgumentNullException>(() => target.LogEvent(EventLevel.Error, null));
        }

        [Test]
        public void LogsTheEventInformation()
        {
            var log = new Mock<ILog>();

            var target = new DefaultEventLoggingPolicy(log.Object);
            target.LogEvent(EventLevel.Error, new PInvokeInt32CallCompletedTraceEvent());

            log.Verify(o => o.Log(EventLevel.Error, It.IsAny<TraceEvent>()), Times.Once);
        }

        [Test]
        public void SwallowExceptionsWhenLoggingEvents()
        {
            var log = new Mock<ILog>();
            log.Setup(o => o.Log(EventLevel.Error, It.IsAny<TraceEvent>())).Throws<Exception>().Verifiable();

            var target = new DefaultEventLoggingPolicy(log.Object);
            target.LogEvent(EventLevel.Error, new PInvokeInt32CallCompletedTraceEvent());

            log.Verify();
        }
    }
}