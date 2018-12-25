using System;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Connections
{
    [TestFixture]
    public class RasClearLinkStatisticsServiceTests
    {
        [Test]
        public void ThrowsAnExceptionWhenApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasClearLinkStatisticsService(null, new Mock<IExceptionPolicy>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasClearLinkStatisticsService(new Mock<IRasApi32>().Object, null));
        }

        [Test]
        public void ThrowsAnExceptionWhenConnectionIsNull()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasClearLinkStatisticsService(api.Object, exceptionPolicy.Object);
            Assert.Throws<ArgumentNullException>(() => target.ClearLinkStatistics(null, 1));
        }

        [Test]
        public void ClearsTheStatisticsAsExpected()
        {
            var handle = new IntPtr(1);

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasClearLinkStatistics(handle, 1)).Returns(SUCCESS).Verifiable();

            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            var target = new RasClearLinkStatisticsService(api.Object, exceptionPolicy.Object);
            target.ClearLinkStatistics(connection.Object, 1);

            api.Verify();
        }

        [Test]
        public void ThrowsAnExceptionWhenTheApiResultIsNonZero()
        {
            var handle = new IntPtr(1);

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasClearLinkStatistics(handle, 1)).Returns(ERROR_INVALID_PARAMETER);

            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            var target = new RasClearLinkStatisticsService(api.Object, exceptionPolicy.Object);
            Assert.Throws<TestException>(() => target.ClearLinkStatistics(connection.Object, 1));

            api.Verify();
        }
    }
}