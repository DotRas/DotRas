using System;
using DotRas.Diagnostics.Events;
using NUnit.Framework;

namespace DotRas.Diagnostics.Formatters
{
    [TestFixture]
    public class PInvokeBoolCallCompletedTraceEventTests
    {
        private PInvokeBoolCallCompletedTraceEventFormatter target;

        [SetUp]
        public void Init()
        {
            target = new PInvokeBoolCallCompletedTraceEventFormatter();
        }

        [Test]
        public void ThrowsAnExceptionWhenEventDataIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => target.Format(null));
        }

        [Test]
        public void FormatsTheEventWithoutFieldsAsExpected()
        {
            var e = new PInvokeBoolCallCompletedTraceEvent
            {
                DllName = "Test",
                MethodName = "Test",
                Duration = TimeSpan.FromSeconds(1),
                Result = true
            };

            var result = target.Format(e);

            Assert.NotNull(result);
        }

        [Test]
        public void FormatsTheEventWithFieldsAsExpected()
        {
            var e = new PInvokeBoolCallCompletedTraceEvent
            {
                DllName = "Test",
                MethodName = "Test",
                Duration = TimeSpan.FromSeconds(1),
                Result = false
            };

            e.Args.Add("Test", "Value");

            var result = target.Format(e);

            Assert.NotNull(result);
        }
    }
}