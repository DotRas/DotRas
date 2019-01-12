using System;
using System.Threading;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Connections
{
    [TestFixture]
    public class RasHangUpTests
    {
        [Test]
        public void ThrowAnExceptionWhenTheApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasHangUpService(null, new Mock<IExceptionPolicy>().Object));
        }

        [Test]
        public void ThrowAnExceptionWhenTheExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasHangUpService(new Mock<IRasApi32>().Object, null));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheConnectionIsNull()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            Assert.Throws<ArgumentNullException>(() => target.HangUp(null, true, CancellationToken.None));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheHandleIsZero()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            Assert.Throws<ArgumentNullException>(() => target.UnsafeHangUp(IntPtr.Zero, true));
        }

        [Test]
        [Timeout(10000)]
        public void HangsUpTheConnectionAsExpected()
        {
            var handle = new IntPtr(1);

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasHangUp(handle)).Returns(ERROR_NO_CONNECTION);

            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            target.HangUp(connection.Object, true, CancellationToken.None);

            api.Verify(o => o.RasHangUp(handle), Times.Once);
        }

        [Test]
        [Timeout(10000)]
        public void HangsUpTheConnectionFromPtrAsExpected()
        {
            var handle = new IntPtr(1);

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasHangUp(handle)).Returns(ERROR_NO_CONNECTION);

            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            target.UnsafeHangUp(handle, true);

            api.Verify(o => o.RasHangUp(handle), Times.Once);
        }

        [Test]
        [Timeout(10000)]
        public void CallsHangUpUntilAllConnectionsHaveBeenClosed()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var handle = new IntPtr(1);

            var counter = 0;
            api.Setup(o => o.RasHangUp(handle)).Returns(() =>
            {
                counter++;
                if (counter < 3)
                {
                    return SUCCESS;
                }
                else
                {
                    return ERROR_NO_CONNECTION;
                }
            });

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            target.UnsafeHangUp(handle, true);

            api.Verify(o => o.RasHangUp(handle), Times.Exactly(3));
        }

        [Test]
        public void ThrowsAnExceptionWhenCancellationIsRequested()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var handle = new IntPtr(1);

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            using (var cancellationSource = new CancellationTokenSource())            
            {
                cancellationSource.Cancel();                

                var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
                Assert.Throws<OperationCanceledException>(() => target.HangUp(connection.Object, true, cancellationSource.Token));
            }
        }

        [Test]
        [Timeout(10000)]
        public void ThrowsAnExceptionWhenHangUpReturnsAnInvalidResultCode()
        {
            var api = new Mock<IRasApi32>();

            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(-1)).Returns(new TestException());

            var handle = new IntPtr(1);

            api.Setup(o => o.RasHangUp(handle)).Returns(-1);

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            Assert.Throws<TestException>(() => target.UnsafeHangUp(handle, true));

            api.Verify(o => o.RasHangUp(handle), Times.Once);
        }
    }
}