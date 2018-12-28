using System;
using DotRas.Diagnostics;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Diagnostics
{
    [TestFixture]
    public class LoggerTests
    {
        private Mock<ILog> log;

        [SetUp]
        public void Init()
        {
            log = new Mock<ILog>();
        }

        [TearDown]
        public void Complete()
        {
            Logger.Clear();
        }

        [Test]
        public void ThrowsAnExceptionWhenValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Logger.SetLocator(null));
        }

        [Test]
        public void ReturnsTheLoggerAsExpected()
        {
            Logger.SetLocator(() => log.Object);

            var actual = Logger.Current;

            Assert.AreSame(log.Object, actual);
        }

        [Test]
        public void ReturnsNullWhenTheLoggerIsNotSet()
        {
            var actual = Logger.Current;

            Assert.IsNull(actual);
        }
    }
}