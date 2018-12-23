using System;
using System.Diagnostics;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Tracing;
using NUnit.Framework;

namespace DotRas.Tests.Diagnostics.Tracing
{
    [TestFixture]
    public class EventLevelConverterTests
    {
        [Test]
        public void ReturnsCriticalAsExpected()
        {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Critical);

            Assert.AreEqual(TraceEventType.Critical, result);
        }

        [Test]
        public void ReturnsErrorAsExpected()
        {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Error);

            Assert.AreEqual(TraceEventType.Error, result);
        }

        [Test]
        public void ReturnsInformationAsExpected()
        {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Information);

            Assert.AreEqual(TraceEventType.Information, result);
        }

        [Test]
        public void ReturnsWarningAsExpected()
        {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Warning);

            Assert.AreEqual(TraceEventType.Warning, result);
        }

        [Test]
        public void ReturnsVerboseAsExpected()
        {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Verbose);

            Assert.AreEqual(TraceEventType.Verbose, result);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheValueIsNotFound()
        {
            var target = new EventLevelConverter();
            Assert.Throws<NotSupportedException>(() => target.Convert((EventLevel)(-1)));
        }
    }
}