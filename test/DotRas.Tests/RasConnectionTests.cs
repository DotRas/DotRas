using System;
using System.Linq;
using System.Threading;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;
using DotRas.Internal.Interop;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasConnectionTests
    {
        private Mock<IServiceProvider> container;

        [SetUp]
        public void Setup()
        {
            container = new Mock<IServiceProvider>();
            Container.Default = container.Object;
        }

        [TearDown]
        public void TearDown()
        {
            Container.Clear();
        }

        [Test]
        public void EnumeratesTheConnectionCorrectly()
        {
            var enumConnections = new Mock<IRasEnumConnections>();
            enumConnections.Setup(o => o.EnumerateConnections()).Returns(new RasConnection[0]);

            container.Setup(o => o.GetService(typeof(IRasEnumConnections))).Returns(enumConnections.Object);
            var result = RasConnection.EnumerateConnections();
            
            Assert.IsNotNull(result);
            container.Verify(o => o.GetService(typeof(IRasEnumConnections)), Times.Once);
            enumConnections.Verify(o => o.EnumerateConnections(), Times.Once);
        }

        [Test]
        public void WillReturnTheCorrectConnectionWhenUsingLinq()
        {
            var connection1 = new Mock<RasConnection>();
            connection1.Setup(o => o.EntryName).Returns("Test1");

            var connection2 = new Mock<RasConnection>();
            connection2.Setup(o => o.EntryName).Returns("Test2");

            var enumConnections = new Mock<IRasEnumConnections>();
            enumConnections.Setup(o => o.EnumerateConnections()).Returns(new[] { connection1.Object, connection2.Object });

            container.Setup(o => o.GetService(typeof(IRasEnumConnections))).Returns(enumConnections.Object);

            var result = RasConnection.EnumerateConnections().SingleOrDefault(o => o.EntryName == "Test2");

            Assert.IsNotNull(result);
            Assert.AreSame(connection2.Object, result);
        }

        [Test]
        public void ReturnTheHandle()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(handle, target.Handle);
        }

        [Test]
        public void ReturnTheDevice()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(device, target.Device);
        }

        [Test]
        public void ReturnTheEntryName()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(entryName, target.EntryName);
        }

        [Test]
        public void ReturnThePhoneBook()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(phoneBook, target.PhoneBookPath);
        }        

        [Test]
        public void ReturnTheSubEntryId()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(subEntryId, target.SubEntryId);
        }

        [Test]
        public void ReturnTheEntryId()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(entryId, target.EntryId);
        }

        [Test]
        public void ReturnTheOptions()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(options, target.Options);
        }

        [Test]
        public void ReturnTheSessionId()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(sessionId, target.SessionId);
        }

        [Test]
        public void ReturnTheCorrelationId()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            Assert.AreEqual(correlationId, target.CorrelationId);
        }

        [Test]
        public void ConstructorThrowsExceptionWhenHandleIsNull()
        {
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(null, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenHandleIsClosed()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            handle.Close();

            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenHandleIsInvalid()
        {
            var handle = RasHandle.FromPtr(new IntPtr(-1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenDeviceIsNull()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, null, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenEntryNameIsNull()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, null, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenEntryNameIsEmpty()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, "", phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenEntryNameIsWhitespace()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var phoneBook = @"C:\Test.pbk";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, "                ", phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenPhoneBookIsNull()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, null, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenPhoneBookIsEmpty()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, "", subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenPhoneBookIsWhitespace()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, "             ", subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void RetrievesTheStatusAsExpected()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var phoneBook = @"C:\Test.pbk";
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var status = new Mock<RasConnectionStatus>();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            rasGetConnectStatus.Setup(o => o.GetConnectionStatus(handle)).Returns(status.Object).Verifiable();

            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            var result = target.GetStatus();

            Assert.AreEqual(status.Object, result);
            rasGetConnectStatus.Verify();
        }

        [Test]
        public void RetrievesTheStatisticsAsExpected()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var phoneBook = @"C:\Test.pbk";
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();
            
            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();

            var rasConnectionStatistics = new Mock<RasConnectionStatistics>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            rasGetConnectionStatistics.Setup(o => o.GetConnectionStatistics(handle)).Returns(rasConnectionStatistics.Object).Verifiable();

            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            var result = target.GetStatistics();

            Assert.AreEqual(rasConnectionStatistics.Object, result);
            rasGetConnectionStatistics.Verify();
        }

        [Test]
        public void HangUpTheConnectionAsExpected()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var phoneBook = @"C:\Test.pbk";
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var cancellationToken = CancellationToken.None;

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);
            target.HangUp(cancellationToken);

            rasHangUp.Verify(o => o.HangUp(handle, cancellationToken), Times.Once);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheHandleIsInvalidDuringGetStatus()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var phoneBook = @"C:\Test.pbk";
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);

            handle.SetHandleAsInvalid();

            Assert.Throws<InvalidHandleException>(() => target.GetStatus());
        }

        [Test]
        public void ThrowsAnExceptionWhenTheHandleIsInvalidDuringGetStatistics()
        {        
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var phoneBook = @"C:\Test.pbk";
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);

            handle.SetHandleAsInvalid();

            Assert.Throws<InvalidHandleException>(() => target.GetStatistics());
        }

        [Test]
        public void ThrowsAnExceptionWhenTheHandleIsInvalidDuringHangUp()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var subEntryId = 1;
            var phoneBook = @"C:\Test.pbk";
            var entryId = Guid.NewGuid();
            var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
            var sessionId = new Luid(1, 1);
            var correlationId = Guid.NewGuid();

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, subEntryId, entryId, options, sessionId, correlationId, rasGetConnectStatus.Object, rasGetConnectionStatistics.Object, rasHangUp.Object);

            handle.SetHandleAsInvalid();

            Assert.Throws<InvalidHandleException>(() => target.HangUp(CancellationToken.None));
        }
    }
}