using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services.Connections {
    [TestFixture]
    public class RasConnectionNotificationServiceTests {
        [Test]
        public void ThrowsAnExceptionWhenApiIsNull() => Assert.Throws<ArgumentNullException>(() => new RasConnectionNotificationService(null, new Mock<IRasConnectionNotificationCallbackHandler>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IRegisteredCallbackFactory>().Object));

        [Test]
        public void ThrowsAnExceptionWhenCallbackHandlerIsNull() => Assert.Throws<ArgumentNullException>(() => new RasConnectionNotificationService(new Mock<IRasApi32>().Object, null, new Mock<IExceptionPolicy>().Object, new Mock<IRegisteredCallbackFactory>().Object));

        [Test]
        public void ThrowsAnExceptionWhenExceptionPolicyIsNull() => Assert.Throws<ArgumentNullException>(() => new RasConnectionNotificationService(new Mock<IRasApi32>().Object, new Mock<IRasConnectionNotificationCallbackHandler>().Object, null, new Mock<IRegisteredCallbackFactory>().Object));

        [Test]
        public void ThrowsAnExceptionWhenCallbackFactoryIsNull() => Assert.Throws<ArgumentNullException>(() => new RasConnectionNotificationService(new Mock<IRasApi32>().Object, new Mock<IRasConnectionNotificationCallbackHandler>().Object, new Mock<IExceptionPolicy>().Object, null));

        [Test]
        public void ThrowsAnExceptionWhenSubscribeAfterDispose() {
            var api = new Mock<IRasApi32>();
            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var callbackFactory = new Mock<IRegisteredCallbackFactory>();

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.Subscribe(new RasNotificationContext()));
        }

        [Test]
        public void ThrowsAnExceptionWhenResetAfterDispose() {
            var api = new Mock<IRasApi32>();
            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var callbackFactory = new Mock<IRegisteredCallbackFactory>();

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.Reset());
        }

        [Test]
        public void ThrowsAnExceptionWhenContextIsNull() {
            var api = new Mock<IRasApi32>();
            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var callbackFactory = new Mock<IRegisteredCallbackFactory>();

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            Assert.Throws<ArgumentNullException>(() => target.Subscribe(null));
        }

        [Test]
        public void ReturnsInactiveAfterCreation() {
            var api = new Mock<IRasApi32>();
            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            var callbackFactory = new Mock<IRegisteredCallbackFactory>();

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            Assert.That(target.IsActive, Is.False);
        }

        [Test]
        public void ShouldRegisterForAndResetConnectedEventsWhenNoHandleIsProvided() {
            var registeredCallback = new Mock<IRegisteredCallback>();

            var api = new Mock<IRasApi32>();
            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackFactory = new Mock<IRegisteredCallbackFactory>();
            callbackFactory.Setup(o => o.Create(It.IsAny<WaitOrTimerCallback>(), It.IsAny<object>())).Returns(registeredCallback.Object);

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            target.Subscribe(
                new RasNotificationContext {
                    Connection = null,
                    OnConnectedCallback = e => { },
                    OnDisconnectedCallback = e => { }
                }
            );

            callbackHandler.Verify(o => o.Initialize(), Times.Once);
            api.Verify(o => o.RasConnectionNotification(INVALID_HANDLE_VALUE, It.IsAny<ISafeHandleWrapper>(), RASCN.Connection), Times.Once);
            api.Verify(o => o.RasConnectionNotification(INVALID_HANDLE_VALUE, It.IsAny<ISafeHandleWrapper>(), RASCN.Disconnection), Times.Once);

            Assert.That(target.IsActive, Is.True);

            target.Reset();

            registeredCallback.Verify(o => o.Dispose(), Times.Exactly(2));
            Assert.That(target.IsActive, Is.False);
        }

        [Test]
        public void ShouldRegisterAndResetForDisconnectedEventsWhenHandleIsProvided() {
            var handle = new IntPtr(1);

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            var registeredCallback = new Mock<IRegisteredCallback>();

            var api = new Mock<IRasApi32>();
            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackFactory = new Mock<IRegisteredCallbackFactory>();
            callbackFactory.Setup(o => o.Create(It.IsAny<WaitOrTimerCallback>(), It.IsAny<object>())).Returns(registeredCallback.Object);

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            target.Subscribe(new RasNotificationContext { Connection = connection.Object, OnDisconnectedCallback = e => { } });

            callbackHandler.Verify(o => o.Initialize(), Times.Once);
            api.Verify(o => o.RasConnectionNotification(INVALID_HANDLE_VALUE, It.IsAny<ISafeHandleWrapper>(), RASCN.Connection), Times.Once);
            api.Verify(o => o.RasConnectionNotification(handle, It.IsAny<ISafeHandleWrapper>(), RASCN.Disconnection), Times.Once);

            Assert.That(target.IsActive, Is.True);

            target.Reset();

            registeredCallback.Verify(o => o.Dispose(), Times.Exactly(2));
            Assert.That(target.IsActive, Is.False);
        }

        [Test]
        public void DisposeMustCleanUpSubscriptions() {
            var handle = new IntPtr(1);

            var connection = new Mock<IRasConnection>();
            connection.Setup(o => o.Handle).Returns(handle);

            var registeredCallback = new Mock<IRegisteredCallback>();

            var api = new Mock<IRasApi32>();
            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackFactory = new Mock<IRegisteredCallbackFactory>();
            callbackFactory.Setup(o => o.Create(It.IsAny<WaitOrTimerCallback>(), It.IsAny<object>())).Returns(registeredCallback.Object);

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            target.Subscribe(new RasNotificationContext { Connection = connection.Object, OnDisconnectedCallback = e => { } });

            target.Dispose();

            registeredCallback.Verify(o => o.Dispose(), Times.Exactly(2));
        }

        [Test]
        public void ThrowsAnExceptionWhenApiResultCodeIsNonZero() {
            var registeredCallback = new Mock<IRegisteredCallback>();

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasConnectionNotification(It.IsAny<IntPtr>(), It.IsAny<ISafeHandleWrapper>(), It.IsAny<RASCN>())).Returns(1);

            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(1)).Throws<TestException>();

            var callbackFactory = new Mock<IRegisteredCallbackFactory>();
            callbackFactory.Setup(o => o.Create(It.IsAny<WaitOrTimerCallback>(), It.IsAny<object>())).Returns(registeredCallback.Object);

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            Assert.Throws<TestException>(() => {
                target.Subscribe(new RasNotificationContext());
            });
        }

        [Test]
        public void ThrowsAnExceptionWhenRegisteredCallbackIsNull() {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasConnectionNotification(It.IsAny<IntPtr>(), It.IsAny<ISafeHandleWrapper>(), It.IsAny<RASCN>())).Returns(1);

            var callbackHandler = new Mock<IRasConnectionNotificationCallbackHandler>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(1)).Throws<TestException>();

            var callbackFactory = new Mock<IRegisteredCallbackFactory>();
            callbackFactory.Setup(o => o.Create(It.IsAny<WaitOrTimerCallback>(), It.IsAny<object>())).Returns((IRegisteredCallback)null);

            var target = new RasConnectionNotificationService(api.Object, callbackHandler.Object, exceptionPolicy.Object, callbackFactory.Object);
            Assert.Throws<InvalidOperationException>(() => {
                target.Subscribe(new RasNotificationContext());
            });
        }
    }
}
