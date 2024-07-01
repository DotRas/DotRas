using System;
using DotRas.Diagnostics.Events;
using NUnit.Framework;

namespace DotRas.Diagnostics.Formatters
{
    [TestFixture]
    public class PInvokeInt32CallCompletedTraceEventFormatterTests
    {
        private PInvokeInt32CallCompletedTraceEventFormatter target;

        [SetUp]
        public void Init()
        {
            target = new PInvokeInt32CallCompletedTraceEventFormatter();
        }

        [Test]
        public void ThrowsAnExceptionWhenEventDataIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => target.Format(null));
        }

        [Test]
        public void FormatsTheEventWithoutFieldsAsExpected()
        {
            var e = new PInvokeInt32CallCompletedTraceEvent
            {
                DllName = "Test",
                MethodName = "Test",
                Duration = TimeSpan.FromSeconds(1),
                Result = 1
            };

            var result = target.Format(e);

            Assert.NotNull(result);
        }

        [Test]
        public void FormatsTheEventWithFieldsAsExpected()
        {
            var e = new PInvokeInt32CallCompletedTraceEvent
            {
                DllName = "Test",
                MethodName = "Test",
                Duration = TimeSpan.FromSeconds(1),
                Result = 1
            };

            e.Args.Add("Test", "Value");
            e.OutArgs.Add("Out", "Result");

            var result = target.Format(e);

            Assert.NotNull(result);
        }
    }
}