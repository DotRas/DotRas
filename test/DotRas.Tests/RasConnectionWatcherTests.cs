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
        public void ReturnsTrueWhenIsActive()
        {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.IsActive).Returns(true);

            var target = new RasConnectionWatcher(api.Object);
            Assert.True(target.IsActive);
        }

        [Test]
        public void ReturnsFalseWhenIsNotActive()
        {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.IsActive).Returns(false);

            var target = new RasConnectionWatcher(api.Object);
            Assert.False(target.IsActive);
        }

        [Test]
        public void ThrowsAnExceptionWhenStartAfterDispose()
        {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.Start());
        }

        [Test]
        public void ThrowsAnExceptionWhenStopAfterDispose()
        {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.Stop());
        }

        [Test]
        public void StartWillSubscribeWithoutAConnection()
        {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>())).Callback<RasNotificationContext>(c =>
            {
                Assert.IsNull(c.Connection);
                Assert.IsNotNull(c.OnConnectedCallback);
                Assert.IsNotNull(c.OnDisconnectedCallback);
            });

            var target = new RasConnectionWatcher(api.Object);
            target.Start();

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
                c.OnConnectedCallback(new RasConnectionEventArgs(new RasConnectionInformation(
                    new IntPtr(1),
                    "Test",
                    "",
                    Guid.NewGuid(),
                    Guid.NewGuid())));
            });

            var target = new RasConnectionWatcher(api.Object);
            target.Connected += (sender, e) =>
            {
                executed = true;
            };

            target.Start();

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
                c.OnDisconnectedCallback(new RasConnectionEventArgs(new RasConnectionInformation(
                    new IntPtr(1),
                    "Test",
                    "",
                    Guid.NewGuid(),
                    Guid.NewGuid())));
            });

            var target = new RasConnectionWatcher(api.Object);
            target.Disconnected += (sender, e) =>
            {
                executed = true;
            };

            target.Start();

            Assert.True(executed);
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

            var target = new RasConnectionWatcher(api.Object)
            {
                Connection = connection.Object
            };

            target.Start();

            api.Verify(o => o.Subscribe(It.IsAny<RasNotificationContext>()), Times.Once);
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
            api.Setup(o => o.IsActive).Returns(true);

            var target = new RasConnectionWatcher(api.Object);
            target.Stop();

            api.Verify(o => o.Reset(), Times.Once);
        }

        [Test]
        public void RestartsTheWatcherWhenChangedWhileActive()
        {
            var connection = new Mock<RasConnection>();

            var api = new Mock<IRasConnectionNotification>();
            api.SetupSequence(o => o.IsActive)
                .Returns(false)
                .Returns(true)
                .Returns(true)
                .Returns(false);

            var target = new RasConnectionWatcher(api.Object);
            target.Start();

            target.Connection = connection.Object;
            
            api.Verify(o => o.Subscribe(It.IsAny<RasNotificationContext>()), Times.Exactly(2));
            api.Verify(o => o.Reset(), Times.Once);
        }
    }
}