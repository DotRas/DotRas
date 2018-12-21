using System;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Tests.Stubs;
using NUnit.Framework;

namespace DotRas.Tests.Diagnostics
{
    [TestFixture]
    public class ConventionBasedEventFormatterFactoryTests
    {
        [Test]
        public void ThrowsAnExceptionWhenTheAttributeDoesNotExist()
        {
            var target = new ConventionBasedEventFormatterFactory();

            var ex = Assert.Throws<FormatterNotFoundException>(() => target.Create<TraceEvent>());
            Assert.AreEqual("The formatter could not be identified.", ex.Message);
            Assert.AreEqual(typeof(TraceEvent), ex.TargetType);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheFormatterIsTheWrongType()
        {
            var target = new ConventionBasedEventFormatterFactory();

            Assert.Throws<InvalidOperationException>(() => target.Create<BadTraceEvent>());
        }

        [Test]
        public void ThrowsAnExceptionWhenTheFormatterCannotBeCreated()
        {
            var target = new ConventionBasedEventFormatterFactory();

            var ex = Assert.Throws<FormatterNotFoundException>(() => target.Create<BadTraceEventWithBadFormatter>());
            Assert.AreEqual("The formatter could not be created. Please verify the formatter contains a parameterless constructor.", ex.Message);
            Assert.AreEqual(typeof(BadTraceEventWithBadFormatter), ex.TargetType);
            Assert.AreEqual(typeof(BadFormatter), ex.FormatterType);
        }

        [Test]
        public void ReturnsTheFormatterAsExpected()
        {
            var target = new ConventionBasedEventFormatterFactory();

            var formatter = target.Create<GoodTraceEventWithGoodFormatter>();
            Assert.IsInstanceOf<GoodFormatter>(formatter);
        }
    }
}