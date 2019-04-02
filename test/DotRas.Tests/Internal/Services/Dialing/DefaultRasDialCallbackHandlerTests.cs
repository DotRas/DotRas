//using System;
//using System.Threading;
//using DotRas.Internal.Abstractions.Factories;
//using DotRas.Internal.Abstractions.Policies;
//using DotRas.Internal.Abstractions.Primitives;
//using DotRas.Internal.Abstractions.Services;
//using DotRas.Internal.Infrastructure.Factories;
//using DotRas.Internal.Services.Dialing;
//using DotRas.Tests.Stubs;
//using Moq;
//using NUnit.Framework;

//namespace DotRas.Tests.Internal.Services.Dialing
//{
//    [TestFixture]
//    public class DefaultRasDialCallbackHandlerTests
//    {
//        [Test]
//        public void ThrowsAnExceptionWhenTheHangUpApiIsNull()
//        {
//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                var unused = new DefaultRasDialCallbackHandler(null, new Mock<IRasEnumConnections>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IValueWaiter<IntPtr>>().Object, new Mock<ITaskCancellationSourceFactory>().Object);
//            });
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenTheRasEnumConnectionsApiIsNull()
//        {
//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                var unused = new DefaultRasDialCallbackHandler(new Mock<IRasHangUp>().Object, null, new Mock<IExceptionPolicy>().Object, new Mock<IValueWaiter<IntPtr>>().Object, new Mock<ITaskCancellationSourceFactory>().Object);
//            });
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
//        {
//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                var unused = new DefaultRasDialCallbackHandler(new Mock<IRasHangUp>().Object, new Mock<IRasEnumConnections>().Object, null, new Mock<IValueWaiter<IntPtr>>().Object, new Mock<ITaskCancellationSourceFactory>().Object);
//            });
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenHandleIsNull()
//        {
//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                var unused = new DefaultRasDialCallbackHandler(new Mock<IRasHangUp>().Object, new Mock<IRasEnumConnections>().Object, new Mock<IExceptionPolicy>().Object, null, new Mock<ITaskCancellationSourceFactory>().Object);
//            });
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenTheCompletionSourceIsNull()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            Assert.Throws<ArgumentNullException>(() => target.Initialize(null, e => { }, () => { }, CancellationToken.None));
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenTheOnStateChangedCallbackIsNull()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            Assert.Throws<ArgumentNullException>(() => target.Initialize(new Mock<ITaskCompletionSource<RasConnection>>().Object, null, () => { }, CancellationToken.None));
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenTheOnCompletedCallbackIsNull()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            Assert.Throws<ArgumentNullException>(() => target.Initialize(new Mock<ITaskCompletionSource<RasConnection>>().Object, e => { }, null, CancellationToken.None));
//        }


//        [Test]
//        public void MustSupportMultipleInitializeForReuseOfHandler()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();

//            var cancellationTokenSource = new Mock<ITaskCancellationSource>();
//            cancellationTokenSource.Setup(o => o.Token).Returns(new CancellationToken());
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();
//            cancellationTokenSourceFactory.Setup(o => o.Create(It.IsAny<CancellationToken>())).Returns(cancellationTokenSource.Object);

//            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
//            var cancellationToken = new CancellationToken(true);

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);

//            target.Initialize(completionSource.Object, (e) => { }, () => { }, cancellationToken);            
//            Assert.IsTrue(target.Initialized);

//            target.Initialize(completionSource.Object, (e) => { }, () => { }, cancellationToken);
//            Assert.IsTrue(target.Initialized);
//        }

//        [Test]
//        public void HangsUpTheConnectionWhenCancelled()
//        {
//            var handle = new IntPtr(1);

//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var cancellationSourceFactory = new TaskCancellationSourceFactory();

//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            waitHandle.Setup(o => o.Value).Returns(handle);

//            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();

//            using (var cts = new CancellationTokenSource())
//            {
//                var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationSourceFactory);
//                target.Initialize(completionSource.Object, (e) => { }, () => { }, cts.Token);

//                cts.Cancel();

//                var result = target.OnCallback(new IntPtr(1), 1, new IntPtr(1), 0, RasConnectionState.OpenPort, 0, 0);
//                Assert.IsFalse(result);
//            }

//            rasHangUp.Verify(o => o.UnsafeHangUp(handle, false), Times.Once);
//            completionSource.Verify(o => o.SetException(It.IsAny<OperationCanceledException>()), Times.Once);
//        }

//        [Test]
//        public void RaisesTheCallbackAction()
//        {
//            var handle = new IntPtr(1);

//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();

//            var cancellationTokenSource = new Mock<ITaskCancellationSource>();
//            cancellationTokenSource.Setup(o => o.Token).Returns(new CancellationToken());
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();
//            cancellationTokenSourceFactory.Setup(o => o.Create(It.IsAny<CancellationToken>())).Returns(cancellationTokenSource.Object);

//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            waitHandle.Setup(o => o.Value).Returns(handle);

//            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
//            var cancellationToken = new CancellationToken();

//            var called = false;

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            target.Initialize(completionSource.Object, (e) =>
//            {
//                Assert.AreEqual(RasConnectionState.OpenPort, e.State);
//                called = true;
//            }, () => { }, cancellationToken);

//            var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.OpenPort, 0, 0);

//            Assert.IsTrue(result);
//            Assert.IsTrue(called);
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenTheDwErrorCodeIsNonZero()
//        {
//            var handle = new IntPtr(1);

//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            exceptionPolicy.Setup(o => o.Create(632)).Returns(new Exception("An exception occurred!")).Verifiable();

//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            waitHandle.Setup(o => o.Value).Returns(handle);

//            var cancellationTokenSource = new Mock<ITaskCancellationSource>();
//            cancellationTokenSource.Setup(o => o.Token).Returns(new CancellationToken());
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();
//            cancellationTokenSourceFactory.Setup(o => o.Create(It.IsAny<CancellationToken>())).Returns(cancellationTokenSource.Object);

//            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
//            var cancellationToken = new CancellationToken();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            target.Initialize(completionSource.Object, (e) => { }, () => { }, cancellationToken);

//            var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.OpenPort, 632, 0);

//            Assert.IsFalse(result);
//            completionSource.Verify(o => o.SetException(It.IsAny<Exception>()), Times.Once);
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenOnCallbackIsNotInitialized()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            Assert.Throws<InvalidOperationException>(() => target.OnCallback(new IntPtr(1), 1, new IntPtr(1), 0, RasConnectionState.OpenPort, 0, 0));
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenTheHandleIsNull()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            Assert.Throws<ArgumentNullException>(() => target.SetHandle(IntPtr.Zero));
//        }

//        [Test]
//        public void SetsTheHandle()
//        {
//            var handle = new IntPtr(1);

//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            target.SetHandle(handle);

//            waitHandle.Verify(o => o.Set(handle), Times.Once);
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenTheHandleIsAlreadySet()
//        {
//            var handle = new IntPtr(1);

//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            waitHandle.Setup(o => o.IsSet).Returns(true);

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            Assert.Throws<InvalidOperationException>(() => target.SetHandle(handle));
//        }

//        [Test]
//        public void ReturnsAnExceptionWhenTheFactoryDoesNotReturnAConnection()
//        {
//            var handle = new IntPtr(1);

//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();

//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            waitHandle.Setup(o => o.Value).Returns(handle);

//            var cancellationTokenSource = new Mock<ITaskCancellationSource>();
//            cancellationTokenSource.Setup(o => o.Token).Returns(new CancellationToken());
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();
//            cancellationTokenSourceFactory.Setup(o => o.Create(It.IsAny<CancellationToken>())).Returns(cancellationTokenSource.Object);

//            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
//            var cancellationToken = new CancellationToken();

//            var target = new StubDefaultRasDialCallbackHandler(_ => null, rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            target.Initialize(completionSource.Object, (e) => { }, () => { }, cancellationToken);

//            var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.Connected, 0, 0);

//            Assert.IsFalse(result);
//            completionSource.Verify(o => o.SetException(It.IsAny<InvalidOperationException>()), Times.Once);
//        }

//        [Test]
//        public void ReturnsTheConnectionWhenConnected()
//        {
//            var handle = new IntPtr(1);

//            var connection = new Mock<RasConnection>();
//            connection.Setup(o => o.Handle).Returns(handle);

//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            rasEnumConnections.Setup(o => o.EnumerateConnections()).Returns(new[]
//            {
//                connection.Object
//            });
//            var exceptionPolicy = new Mock<IExceptionPolicy>();

//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();
//            waitHandle.Setup(o => o.Value).Returns(handle);

//            var cancellationTokenSource = new Mock<ITaskCancellationSource>();
//            cancellationTokenSource.Setup(o => o.Token).Returns(new CancellationToken());
//            var cancellationTokenSourceFactory = new Mock<ITaskCancellationSourceFactory>();
//            cancellationTokenSourceFactory.Setup(o => o.Create(It.IsAny<CancellationToken>())).Returns(cancellationTokenSource.Object);

//            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
//            var cancellationToken = new CancellationToken();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationTokenSourceFactory.Object);
//            target.Initialize(completionSource.Object, (e) => { }, () => { }, cancellationToken);

//            var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.Connected, 0, 0);

//            Assert.IsFalse(result);
//            completionSource.Verify(o => o.SetResult(It.IsAny<RasConnection>()), Times.Once);
//        }

//        [Test]
//        public void DisposesTheHandleDuringDispose()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();

//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();

//            var cancellationSourceFactory = new Mock<ITaskCancellationSourceFactory>();

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationSourceFactory.Object);
//            target.Dispose();

//            waitHandle.Verify(o => o.Dispose(), Times.Once);
//        }

//        [Test]
//        public void DisposesTheCancellationSourceDuringDispose()
//        {
//            var rasHangUp = new Mock<IRasHangUp>();
//            var rasEnumConnections = new Mock<IRasEnumConnections>();
//            var exceptionPolicy = new Mock<IExceptionPolicy>();
//            var waitHandle = new Mock<IValueWaiter<IntPtr>>();

//            var cancellationSource = new Mock<ITaskCancellationSource>();

//            var cancellationSourceFactory = new Mock<ITaskCancellationSourceFactory>();
//            cancellationSourceFactory.Setup(o => o.Create(It.IsAny<CancellationToken>())).Returns(cancellationSource.Object);

//            var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object, cancellationSourceFactory.Object);
//            target.Initialize(new Mock<ITaskCompletionSource<RasConnection>>().Object, e => { }, () => { }, CancellationToken.None);

//            target.Dispose();

//            cancellationSource.Verify(o => o.Dispose(), Times.Once);
//        }
//    }
//}