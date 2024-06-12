using DotRas.Diagnostics;
using DotRas.Diagnostics.Tracing;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace DotRas.Tests.Diagnostics.Tracing {
    [TestFixture]
    public class EventLevelConverterTests {
        [Test]
        public void ReturnsCriticalAsExpected() {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Critical);

            Assert.That(result, Is.EqualTo(TraceEventType.Critical));
        }

        [Test]
        public void ReturnsErrorAsExpected() {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Error);

            Assert.That(result, Is.EqualTo(TraceEventType.Error));
        }

        [Test]
        public void ReturnsInformationAsExpected() {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Information);

            Assert.That(result, Is.EqualTo(TraceEventType.Information));
        }

        [Test]
        public void ReturnsWarningAsExpected() {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Warning);

            Assert.That(result, Is.EqualTo(TraceEventType.Warning));
        }

        [Test]
        public void ReturnsVerboseAsExpected() {
            var target = new EventLevelConverter();
            var result = target.Convert(EventLevel.Verbose);

            Assert.That(result, Is.EqualTo(TraceEventType.Verbose));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheValueIsNotFound() {
            var target = new EventLevelConverter();
            Assert.Throws<NotSupportedException>(() => target.Convert((EventLevel)(-1)));
        }
    }
}
