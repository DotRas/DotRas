using DotRas.Internal.Abstractions.Services;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using System;

namespace DotRas.Tests {
    [TestFixture]
    public class RasConnectionWatcherTests {
        [Test]
        public void DoesNotThrowAnExceptionWhenCreatingTheObject() => Assert.DoesNotThrow(() => new RasConnectionWatcher());

        [Test]
        public void ThrowsAnExceptionWhenTheApiIsNull() => Assert.Throws<ArgumentNullException>(() => new RasConnectionWatcher(null));

        [Test]
        public void RaisesTheErrorEventWhenAnErrorOccursDuringConnectedEvent() {
            var called = false;

            var api = new Mock<IRasConnectionNotification>();

            var target = new TestableRasConnectionWatcher(api.Object);
            target.Connected += (sender, e) => {
                throw new TestException();
            };

            target.Error += (sender, e) => {
                Assert.That(e.GetException(), Is.InstanceOf<TestException>());
                called = true;
            };

            target.RaiseConnectedEvent(new RasConnectionEventArgs(new RasConnectionInformation(IntPtr.Zero, "", "", Guid.Empty, Guid.Empty)));

            Assert.That(called, Is.True, "The event was not called as expected.");
        }

        [Test]
        public void RaisesTheErrorEventWhenAnErrorOccursDuringDisconnectedEvent() {
            var called = false;

            var api = new Mock<IRasConnectionNotification>();

            var target = new TestableRasConnectionWatcher(api.Object);
            target.Disconnected += (sender, e) => {
                throw new TestException();
            };

            target.Error += (sender, e) => {
                Assert.That(e.GetException(), Is.InstanceOf<TestException>());
                called = true;
            };

            target.RaiseDisconnectedEvent(new RasConnectionEventArgs(new RasConnectionInformation(IntPtr.Zero, "", "", Guid.Empty, Guid.Empty)));

            Assert.That(called, Is.True, "The event was not called as expected.");
        }

        [Test]
        public void ThrowsAnExceptionWhenConnectionChangedAfterDisposed() {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.IsActive).Returns(true);

            var connection = new Mock<IRasConnection>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.Connection = connection.Object);
        }

        [Test]
        public void ReturnsTrueWhenIsActive() {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.IsActive).Returns(true);

            var target = new RasConnectionWatcher(api.Object);
            Assert.That(target.IsActive, Is.True);
        }

        [Test]
        public void ReturnsFalseWhenIsNotActive() {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.IsActive).Returns(false);

            var target = new RasConnectionWatcher(api.Object);
            Assert.That(target.IsActive, Is.False);
        }

        [Test]
        public void ThrowsAnExceptionWhenStartAfterDispose() {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.Start());
        }

        [Test]
        public void ThrowsAnExceptionWhenStopAfterDispose() {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            Assert.Throws<ObjectDisposedException>(() => target.Stop());
        }

        [Test]
        public void StartWillSubscribeWithoutAConnection() {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>()))
                .Callback<RasNotificationContext>(c => {
                    Assert.Multiple(() => {
                        Assert.That(c.Connection, Is.Null);
                        Assert.That(c.OnConnectedCallback, Is.Not.Null);
                        Assert.That(c.OnDisconnectedCallback, Is.Not.Null);
                    });
                });

            var target = new RasConnectionWatcher(api.Object);
            target.Start();

            api.Verify(o => o.Subscribe(It.IsAny<RasNotificationContext>()), Times.Once);
        }

        [Test]
        public void OnConnectedCallbackMustRaiseTheConnectedEvent() {
            var executed = false;

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>()))
                .Callback<RasNotificationContext>(c => {
                    Assert.Throws<ArgumentNullException>(() => c.OnConnectedCallback(null));
                    c.OnConnectedCallback(new RasConnectionEventArgs(new RasConnectionInformation(new IntPtr(1), "Test", "", Guid.NewGuid(), Guid.NewGuid())));
                });

            var target = new RasConnectionWatcher(api.Object);
            target.Connected += (sender, e) => {
                executed = true;
            };

            target.Start();

            Assert.That(executed, Is.True);
        }

        [Test]
        public void OnDisconnectedCallbackMustRaiseTheDisconnectedEvent() {
            var executed = false;

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>()))
                .Callback<RasNotificationContext>(c => {
                    Assert.Throws<ArgumentNullException>(() => c.OnDisconnectedCallback(null));
                    c.OnDisconnectedCallback(new RasConnectionEventArgs(new RasConnectionInformation(new IntPtr(1), "Test", "", Guid.NewGuid(), Guid.NewGuid())));
                });

            var target = new RasConnectionWatcher(api.Object);
            target.Disconnected += (sender, e) => {
                executed = true;
            };

            target.Start();

            Assert.That(executed, Is.True);
        }

        [Test]
        public void WatchConnectionWillSubscribeWithConnection() {
            var connection = new Mock<RasConnection>();

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>()))
                .Callback<RasNotificationContext>(c => {
                    Assert.Multiple(() => {
                        Assert.That(c.Connection, Is.EqualTo(connection.Object));
                        Assert.That(c.OnConnectedCallback, Is.Not.Null);
                        Assert.That(c.OnDisconnectedCallback, Is.Not.Null);
                    });
                });

            var target = new RasConnectionWatcher(api.Object) { Connection = connection.Object };

            target.Start();

            api.Verify(o => o.Subscribe(It.IsAny<RasNotificationContext>()), Times.Once);
        }

        [Test]
        public void DisposeWillDisposeTheApi() {
            var api = new Mock<IRasConnectionNotification>();

            var target = new RasConnectionWatcher(api.Object);
            target.Dispose();

            api.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public void StopWillResetTheApi() {
            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.IsActive).Returns(true);

            var target = new RasConnectionWatcher(api.Object);
            target.Stop();

            api.Verify(o => o.Reset(), Times.Once);
        }

        [Test]
        public void RestartsTheWatcherWhenChangedWhileActive() {
            var connection = new Mock<RasConnection>();

            var api = new Mock<IRasConnectionNotification>();
            api.SetupSequence(o => o.IsActive).Returns(false).Returns(true).Returns(true).Returns(false);

            var target = new RasConnectionWatcher(api.Object);
            target.Start();

            target.Connection = connection.Object;

            api.Verify(o => o.Subscribe(It.IsAny<RasNotificationContext>()), Times.Exactly(2));
            api.Verify(o => o.Reset(), Times.Once);
        }

        [Test]
        public void RaisesTheErrorEventWhenAnErrorOccursWithinConnectedEvent() {
            Action<RasConnectionEventArgs> onConnectedCallback = null;
            bool executed = false;

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>()))
                .Callback<RasNotificationContext>(context => {
                    onConnectedCallback = context.OnConnectedCallback;
                });

            var target = new RasConnectionWatcher(api.Object);
            target.Connected += (sender, e) => {
                throw new TestException();
            };
            target.Error += (sender, e) => {
                executed = true;
            };

            target.Start();

            Assert.That(onConnectedCallback, Is.Not.Null);

            var eventData = new Mock<RasConnectionEventArgs>();
            onConnectedCallback(eventData.Object);

            Assert.That(executed, Is.True, "The error event was not executed as expected.");
        }

        [Test]
        public void RaisesTheErrorEventWhenAnErrorOccursWithinDisconnectedEvent() {
            Action<RasConnectionEventArgs> onDisconnectedCallback = null;
            bool executed = false;

            var api = new Mock<IRasConnectionNotification>();
            api.Setup(o => o.Subscribe(It.IsAny<RasNotificationContext>()))
                .Callback<RasNotificationContext>(context => {
                    onDisconnectedCallback = context.OnDisconnectedCallback;
                });

            var target = new RasConnectionWatcher(api.Object);
            target.Disconnected += (sender, e) => {
                throw new TestException();
            };
            target.Error += (sender, e) => {
                executed = true;
            };

            target.Start();

            Assert.That(onDisconnectedCallback, Is.Not.Null);

            var eventData = new Mock<RasConnectionEventArgs>();
            onDisconnectedCallback(eventData.Object);

            Assert.That(executed, Is.True, "The error event was not executed as expected.");
        }
    }
}
