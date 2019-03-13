using System;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.Connections;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services.Connections
{
    [TestFixture]
    public class RasConnectionNotificationCallbackHandlerTests
    {
        [Test]
        public void ThrowsAnExceptionWhenEnumConnectionsServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasConnectionNotificationCallbackHandler(null));
        }

        [Test]
        public void MustInitializeTheCurrentState()
        {
            var rasEnumConnections = new Mock<IRasEnumConnections>();

            var target = new RasConnectionNotificationCallbackHandler(rasEnumConnections.Object);
            target.Initialize();

            rasEnumConnections.Verify(o => o.EnumerateConnections(), Times.Once);
        }

        [Test]
        public void DoesNotThrowAnExceptionWhenInitializedMoreThanOnce()
        {
            var rasEnumConnections = new Mock<IRasEnumConnections>();

            var target = new RasConnectionNotificationCallbackHandler(rasEnumConnections.Object);

            target.Initialize();
            Assert.DoesNotThrow(() => target.Initialize());

            rasEnumConnections.Verify(o => o.EnumerateConnections(), Times.Once);
        }

        [Test]
        public void DoesNotThrowAnErrorWhenTheObjIsNull()
        {
            var rasEnumConnections = new Mock<IRasEnumConnections>();

            var target = new RasConnectionNotificationCallbackHandler(rasEnumConnections.Object);
            target.OnCallback(null, false);

            rasEnumConnections.Verify(o => o.EnumerateConnections(), Times.Never);
        }

        [Test]
        public void ExecutesTheCallbackWhenAConnectionHasDisconnected()
        {
            var executed = false;
            var connection = new Mock<RasConnection>();
            var registeredCallback = new Mock<IRegisteredCallback>();

            var rasEnumConnections = new Mock<IRasEnumConnections>();
            rasEnumConnections.SetupSequence(o => o.EnumerateConnections())
                .Returns(new[] { connection.Object })
                .Returns(new RasConnection[0]);

            var target = new RasConnectionNotificationCallbackHandler(rasEnumConnections.Object);
            target.Initialize();

            target.OnCallback(new RasConnectionNotificationStateObject
            {
                Callback = (e) =>
                {
                    executed = true;
                },
                NotificationType = RASCN.Disconnection,
                RegisteredCallback = registeredCallback.Object
            }, false);

            Assert.True(executed);
            rasEnumConnections.Verify(o => o.EnumerateConnections(), Times.Exactly(2));
        }

        [Test]
        public void ExecutesTheCallbackWhenAConnectionHasConnected()
        {
            var executed = false;
            var connection = new Mock<RasConnection>();
            var registeredCallback = new Mock<IRegisteredCallback>();

            var rasEnumConnections = new Mock<IRasEnumConnections>();
            rasEnumConnections.SetupSequence(o => o.EnumerateConnections())
                .Returns(new RasConnection[0])
                .Returns(new[] { connection.Object });

            var target = new RasConnectionNotificationCallbackHandler(rasEnumConnections.Object);
            target.Initialize();

            target.OnCallback(new RasConnectionNotificationStateObject
            {
                Callback = (e) =>
                {
                    executed = true;
                },
                NotificationType = RASCN.Connection,
                RegisteredCallback = registeredCallback.Object
            }, false);

            Assert.True(executed);
            rasEnumConnections.Verify(o => o.EnumerateConnections(), Times.Exactly(2));
        }
    }
}