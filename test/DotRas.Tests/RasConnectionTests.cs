using System;
using System.Linq;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;
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
        public void ReturnTheEntryName()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            Assert.AreEqual(entryName, target.EntryName);
        }

        [Test]
        public void ReturnTheHandle()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            Assert.AreEqual(handle, target.Handle);
        }

        [Test]
        public void ReturnTheDevice()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            Assert.AreEqual(device, target.Device);
        }

        [Test]
        public void ReturnThePhoneBook()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            var target = new RasConnection(handle, device, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            Assert.AreEqual(phoneBook, target.PhoneBookPath);
        }

        [Test]
        public void ConstructorThrowsExceptionWhenHandleIsNull()
        {
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(null, device, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
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

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenHandleIsInvalid()
        {
            var handle = RasHandle.FromPtr(new IntPtr(-1));
            var device = new TestDevice("Test");
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenDeviceIsNull()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var entryName = "Test";
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, null, entryName, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenEntryNameIsNull()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, null, phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenEntryNameIsEmpty()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, "", phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenEntryNameIsWhitespace()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var phoneBook = @"C:\Test.pbk";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, "                ", phoneBook, rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenPhoneBookIsNull()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, null, rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenPhoneBookIsEmpty()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, "", rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }

        [Test]
        public void ConstructorThrowsExceptionWhenPhoneBookIsWhitespace()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));
            var device = new TestDevice("Test");
            var entryName = "Test";

            var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();
            var rasHangUp = new Mock<IRasHangUp>();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasConnection(handle, device, entryName, "             ", rasGetConnectStatus.Object, rasHangUp.Object);
            });
        }
    }
}