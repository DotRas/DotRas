using System;
using DotRas.Diagnostics.Events;
using NUnit.Framework;

namespace DotRas.Diagnostics.Formatters
{
    [TestFixture]
    public class RasDialCallbackCompletedTraceEventFormatterTests
    {
        private RasDialCallbackCompletedTraceEventFormatter target;

        [SetUp]
        public void Init()
        {
            target = new RasDialCallbackCompletedTraceEventFormatter();
        }

        [Test]
        public void ThrowsAnExceptionWhenEventDataIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => target.Format(null));
        }

        [Test]
        public void FormatsTheEventWithoutFieldsAsExpected()
        {
            var e = new RasDialCallbackCompletedTraceEvent
            {
                OccurredOn = DateTime.Now,
                Result = true
            };

            var result = target.Format(e);

            Assert.NotNull(result);
        }

        [Test]
        public void FormatsTheEventWithFieldsAsExpected()
        {
            var e = new RasDialCallbackCompletedTraceEvent
            {
                OccurredOn = DateTime.Now,
                Result = true
            };

            e.Args.Add("Test", "Value");

            var result = target.Format(e);

            Assert.NotNull(result);
        }
    }
}