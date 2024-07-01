using System;
using DotRas.Diagnostics.Events;
using NUnit.Framework;

namespace DotRas.Diagnostics.Formatters
{
    [TestFixture]
    public class StructMarshalledToPtrTraceEventFormatterTests
    {
        private StructMarshalledToPtrTraceEventFormatter target;

        [SetUp]
        public void Init() 
        {
            target = new StructMarshalledToPtrTraceEventFormatter();
        }

        [Test]
        public void ThrowsAnExceptionWhenEventDataIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => target.Format(null));
        }

        [Test]
        public void FormatsTheEventWithoutFieldsAsExpected()
        {
            var e = new StructMarshalledToPtrTraceEvent
            {
                StructureType = typeof(object),
                Result = IntPtr.Zero,
                Duration = TimeSpan.FromSeconds(1)
            };

            var result = target.Format(e);

            Assert.NotNull(result);
        }

        [Test]
        public void FormatsTheEventWithFieldsAsExpected()
        {
            var e = new StructMarshalledToPtrTraceEvent
            {
                StructureType = typeof(object),
                Result = IntPtr.Zero,
                Duration = TimeSpan.FromSeconds(1)
            };

            e.Fields.Add("Test", "Value");

            var result = target.Format(e);

            Assert.NotNull(result);
        }
    }
}