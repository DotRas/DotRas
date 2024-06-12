using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using System;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Connections {
    [TestFixture]
    public class RasGetConnectionStatisticsServiceTests {
        private delegate int RasGetConnectionStatisticsCallback(IntPtr handle, ref RAS_STATS statistics);

        [Test]
        public void ThrowsAnExceptionWhenApiIsNull() => Assert.Throws<ArgumentNullException>(() => new RasGetConnectionStatisticsService(null, new Mock<IStructFactory>().Object, new Mock<IExceptionPolicy>().Object));

        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull() => Assert.Throws<ArgumentNullException>(() => new RasGetConnectionStatisticsService(new Mock<IRasApi32>().Object, null, new Mock<IExceptionPolicy>().Object));

        [Test]
        public void ThrowsAnExceptionWhenExceptionPolicyIsNull() => Assert.Throws<ArgumentNullException>(() => new RasGetConnectionStatisticsService(new Mock<IRasApi32>().Object, new Mock<IStructFactory>().Object, null));

        [Test]
        public void ThrowsAnExceptionWhenHandleIsNull() {
            var api = new Mock<IRasApi32>();
            var structFactory = new Mock<IStructFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasGetConnectionStatisticsService(api.Object, structFactory.Object, exceptionPolicy.Object);
            Assert.Throws<ArgumentNullException>(() => target.GetConnectionStatistics(null));
        }

        [Test]
        public void ReturnsTheStatisticsAsExpected() {
            var handle = new IntPtr(1);
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasGetConnectionStatistics(handle, ref It.Ref<RAS_STATS>.IsAny))
                .Returns(
                    new RasGetConnectionStatisticsCallback(
                        (IntPtr h, ref RAS_STATS stats) => {
                            stats.dwBytesXmited = 1;
                            stats.dwBytesRcved = 2;
                            stats.dwFramesXmited = 3;
                            stats.dwFramesRcved = 4;
                            stats.dwCrcErr = 5;
                            stats.dwTimeoutErr = 6;
                            stats.dwAlignmentErr = 7;
                            stats.dwHardwareOverrunErr = 8;
                            stats.dwFramingErr = 9;
                            stats.dwBufferOverrunErr = 10;
                            stats.dwCompressionRatioIn = 11;
                            stats.dwCompressionRatioOut = 12;
                            stats.dwBps = 13;
                            stats.dwConnectDuration = 14;

                            return SUCCESS;
                        }
                    )
                );

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            var structFactory = new Mock<IStructFactory>();
            structFactory.Setup(o => o.Create<RAS_STATS>()).Returns(new RAS_STATS());

            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasGetConnectionStatisticsService(api.Object, structFactory.Object, exceptionPolicy.Object);
            var result = target.GetConnectionStatistics(connection.Object);

            Assert.Multiple(() => {
                Assert.That(result.BytesTransmitted, Is.EqualTo(1));
                Assert.That(result.BytesReceived, Is.EqualTo(2));
                Assert.That(result.FramesTransmitted, Is.EqualTo(3));
                Assert.That(result.FramesReceived, Is.EqualTo(4));
                Assert.That(result.CrcErrors, Is.EqualTo(5));
                Assert.That(result.TimeoutErrors, Is.EqualTo(6));
                Assert.That(result.AlignmentErrors, Is.EqualTo(7));
                Assert.That(result.HardwareOverrunErrors, Is.EqualTo(8));
                Assert.That(result.FramingErrors, Is.EqualTo(9));
                Assert.That(result.BufferOverrunErrors, Is.EqualTo(10));
                Assert.That(result.CompressionRatioIn, Is.EqualTo(11));
                Assert.That(result.CompressionRatioOut, Is.EqualTo(12));
                Assert.That(result.LinkSpeed, Is.EqualTo(13));
                Assert.That(result.ConnectionDuration.TotalMilliseconds, Is.EqualTo(14));
            });
        }

        [Test]
        public void ThrowsAnExceptionWhenTheApiResultIsNonZero() {
            var handle = new IntPtr(1);
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasGetConnectionStatistics(handle, ref It.Ref<RAS_STATS>.IsAny)).Returns(ERROR_INVALID_PARAMETER);

            var structFactory = new Mock<IStructFactory>();
            structFactory.Setup(o => o.Create<RAS_STATS>()).Returns(new RAS_STATS());

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

            var target = new RasGetConnectionStatisticsService(api.Object, structFactory.Object, exceptionPolicy.Object);
            Assert.Throws<TestException>(() => target.GetConnectionStatistics(connection.Object));
        }
    }
}
