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

namespace DotRas.Tests.Internal.Services
{
    [TestFixture]
    public class RasHangUpTests
    {
        [Test]
        public void ThrowAnExceptionWhenTheApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasHangUp(null, new Mock<IExceptionPolicy>().Object));
        }

        [Test]
        public void ThrowAnExceptionWhenTheExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasHangUp(new Mock<IRasApi32>().Object, null));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheHandleIsNull()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasHangUp(api.Object, exceptionPolicy.Object);
            Assert.Throws<ArgumentNullException>(() => target.HangUp(null, CancellationToken.None));
        }

        [Test]
        public void ThrowsAnExceptionWhenTheHandleIsInvalidOrClosed()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            using (var handle = new RasHandle())
            {
                var target = new RasHangUp(api.Object, exceptionPolicy.Object);
                Assert.Throws<ArgumentException>(() => target.HangUp(handle, CancellationToken.None));
            }
        }

        [Test]
        [Timeout(10000)]
        public void CallsHangUpUntilAllConnectionsHaveBeenClosed()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            using (var handle = RasHandle.FromPtr(new IntPtr(1)))
            {
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

                var target = new RasHangUp(api.Object, exceptionPolicy.Object);
                target.HangUp(handle, CancellationToken.None);

                api.Verify(o => o.RasHangUp(handle), Times.Exactly(3));
            }
        }

        [Test]
        public void ThrowsAnExceptionWhenCancellationIsRequested()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            using (var cancellationSource = new CancellationTokenSource())
            using (var handle = RasHandle.FromPtr(new IntPtr(1)))
            {
                cancellationSource.Cancel();                

                var target = new RasHangUp(api.Object, exceptionPolicy.Object);
                Assert.Throws<OperationCanceledException>(() => target.HangUp(handle, cancellationSource.Token));
            }
        }

        [Test]
        [Timeout(10000)]
        public void ThrowsAnExceptionWhenHangUpReturnsAnInvalidResultCode()
        {
            var api = new Mock<IRasApi32>();

            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(-1)).Returns(new TestException());

            using (var handle = RasHandle.FromPtr(new IntPtr(1)))
            {
                api.Setup(o => o.RasHangUp(handle)).Returns(-1);

                var target = new RasHangUp(api.Object, exceptionPolicy.Object);
                Assert.Throws<TestException>(() => target.HangUp(handle, CancellationToken.None));

                api.Verify(o => o.RasHangUp(handle), Times.Once);
            }
        }
    }
}