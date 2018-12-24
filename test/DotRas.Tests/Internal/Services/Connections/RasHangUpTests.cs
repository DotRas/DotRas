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
        public void ThrowsAnExceptionWhenTheHandleIsNull()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            Assert.Throws<ArgumentNullException>(() => target.HangUp(null, CancellationToken.None));
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
            target.HangUp(handle, CancellationToken.None);

            api.Verify(o => o.RasHangUp(handle), Times.Exactly(3));
        }

        [Test]
        public void ThrowsAnExceptionWhenCancellationIsRequested()
        {
            var api = new Mock<IRasApi32>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var handle = new IntPtr(1);

            using (var cancellationSource = new CancellationTokenSource())            
            {
                cancellationSource.Cancel();                

                var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
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

            var handle = new IntPtr(1);

            api.Setup(o => o.RasHangUp(handle)).Returns(-1);

            var target = new RasHangUpService(api.Object, exceptionPolicy.Object);
            Assert.Throws<TestException>(() => target.HangUp(handle, CancellationToken.None));

            api.Verify(o => o.RasHangUp(handle), Times.Once);
        }
    }
}