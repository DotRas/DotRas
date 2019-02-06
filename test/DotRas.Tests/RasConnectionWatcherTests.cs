using System;
using DotRas.Internal.Abstractions.Services;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasConnectionWatcherTests
    {
        [Test]
        public void DoesNotThrowAnExceptionWhenCreatingTheObject()
        {
            Assert.DoesNotThrow(() => new RasConnectionWatcher());
        }

        [Test]
        public void ThrowsAnExceptionWhenTheApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasConnectionWatcher(null));
        }

        [Test]
        public void ReturnsTrueWhenTheSubscriptionsIsGreaterThanZero()
        {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.SubscriptionsCount).Returns(1);

            var target = new RasConnectionWatcher(api.Object);
            Assert.True(target.IsActive);
        }

        [Test]
        public void ReturnsFalseWhenTheSubscriptionsIsZero()
        {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.SubscriptionsCount).Returns(0);

            var target = new RasConnectionWatcher(api.Object);
            Assert.False(target.IsActive);
        }

        [Test]
        public void ThrowsAnExceptionWhenWatchAnyConnectionsAfterDispose()
        {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.WatchAnyConnections());
        }

        [Test]
        public void WatchAnyConnectionsWillSubscribeWithoutAConnection()
        {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>())).Callback<RasNotificationContext>(c =>
            {
                Assert.IsNull(c.Connection);
                Assert.IsNotNull(c.OnConnectedCallback);
                Assert.IsNotNull(c.OnDisconnectedCallback);
            });

            var target = new RasConnectionWatcher(api.Object);
            target.WatchAnyConnections();

            api.Verify(o => o.Subscribe(It.IsAny<RasNotificationContext>()), Times.Once);
        }

        [Test]
        public void OnConnectedCallbackMustRaiseTheConnectedEvent()
        {
            var executed = false;

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>())).Callback<RasNotificationContext>(c =>
            {
                Assert.Throws<ArgumentNullException>(() => c.OnConnectedCallback(null));
                c.OnConnectedCallback(new RasConnectionEventArgs(new Mock<RasConnection>().Object));
            });

            var target = new RasConnectionWatcher(api.Object);
            target.Connected += (sender, e) =>
            {
                executed = true;
            };

            target.WatchAnyConnections();

            Assert.True(executed);
        }

        [Test]
        public void OnDisconnectedCallbackMustRaiseTheDisconnectedEvent()
        {
            var executed = false;

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>())).Callback<RasNotificationContext>(c =>
            {
                Assert.Throws<ArgumentNullException>(() => c.OnDisconnectedCallback(null));
                c.OnDisconnectedCallback(new RasConnectionEventArgs(new Mock<RasConnection>().Object));
            });

            var target = new RasConnectionWatcher(api.Object);
            target.Disconnected += (sender, e) =>
            {
                executed = true;
            };

            target.WatchAnyConnections();

            Assert.True(executed);
        }

        [Test]
        public void ThrowsAnExceptionWhenConnectionIsNull()
        {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            Assert.Throws<ArgumentNullException>(() => target.WatchConnection(null));
        }

        [Test]
        public void WatchConnectionWillSubscribeWithConnection()
        {
            var connection = new Mock<RasConnection>();

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>())).Callback<RasNotificationContext>(c =>
            {
                Assert.AreEqual(connection.Object, c.Connection);
                Assert.IsNotNull(c.OnConnectedCallback);
                Assert.IsNotNull(c.OnDisconnectedCallback);
            });

            var target = new RasConnectionWatcher(api.Object);
            target.WatchConnection(connection.Object);

            api.Verify(o => o.Subscribe(It.IsAny<RasNotificationContext>()), Times.Once);
        }

        [Test]
        public void ThrowsAnExceptionWhenWatchConnectionsAfterDispose()
        {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.WatchConnection(new Mock<RasConnection>().Object));
        }

        [Test]
        public void DisposeWillDisposeTheApi()
        {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            api.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public void StopWillResetTheApi()
        {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Stop();

            api.Verify(o => o.Reset(), Times.Once);
        }
    }
}