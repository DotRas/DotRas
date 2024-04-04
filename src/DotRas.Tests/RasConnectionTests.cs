﻿using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests;

[TestFixture]
public class RasConnectionTests
{
    private Mock<IServiceProvider> services;

    [SetUp]
    public void Setup()
    {
        services = new Mock<IServiceProvider>();
        ServiceLocator.SetLocator(() => services.Object);
    }

    [TearDown]
    public void TearDown()
    {
        ServiceLocator.Reset();
    }

    [Test]
    public void EqualsTheHashCodeOfTheHandle()
    {
        var handle = new IntPtr(1);

        var target = new Mock<RasConnection>();
        target.Setup(o => o.Handle).Returns(handle);
        target.Setup(o => o.GetHashCode()).CallBase();

        var actual = target.Object.GetHashCode();

        Assert.AreEqual(handle.GetHashCode(), actual);
    }

    [Test]
    public void DoesNotEqualNullWhenUsingEqualsWithObject()
    {
        object other = null;
        var target = new Mock<RasConnection>();
        target.Setup(o => o.Equals(other)).CallBase();

        Assert.False(target.Object.Equals((object)null));
    }

    [Test]
    public void EqualsTheOtherConnection()
    {
        var target = new Mock<RasConnection>();
        target.Setup(o => o.Handle).Returns(new IntPtr(1));

        var other = new Mock<RasConnection>();
        other.Setup(o => o.Handle).Returns(new IntPtr(1));

        target.Setup(o => o.Equals(other.Object)).CallBase();

        Assert.True(target.Object == other.Object);
    }

    [Test]
    public void EqualsTheOtherConnectionWhenOtherIsAnObject()
    {
        var target = new Mock<RasConnection>();
        target.Setup(o => o.Handle).Returns(new IntPtr(1));

        var other = new Mock<RasConnection>();
        other.Setup(o => o.Handle).Returns(new IntPtr(1));

        target.Setup(o => o.Equals(other.Object)).CallBase();

        object otherTarget = other.Object;
        target.Setup(o => o.Equals(otherTarget)).CallBase();

        Assert.True(target.Object.Equals(otherTarget));
    }

    [Test]
    public void DoesNotEqualTheOtherConnection()
    {
        var target = new Mock<RasConnection>();
        target.Setup(o => o.Handle).Returns(new IntPtr(1));

        var other = new Mock<RasConnection>();
        other.Setup(o => o.Handle).Returns(new IntPtr(2));

        Assert.True(target.Object != other.Object);
    }

    [Test]
    public void DoesEqualWhenBothAreNull()
    {
        RasConnection targetA = null;
        RasConnection targetB = null;

        Assert.True(targetA == targetB);
    }

    [Test]
    public void DoesNotEqualWhenOneIsExpectedToBeNull()
    {
        var target = new Mock<RasConnection>();

        Assert.False(target.Object == null);
    }

    [Test]
    public void DoesNotEqualWhenOtherIsExpectedToBeNullUsingEquals()
    {
        var target = new Mock<RasConnection>();

        RasConnection other = null;
        target.Setup(o => o.Equals(other)).CallBase();

        Assert.False(target.Object.Equals(null));
    }

    [Test]
    public void DoesNotEqualWhenOneIsExpectedToBeNullWhenUsingYodaSyntax()
    {
        var target = new Mock<RasConnection>();

        Assert.False(null == target.Object);
    }

    [Test]
    public void EnumeratesTheConnectionCorrectly()
    {
        var enumConnections = new Mock<IRasEnumConnections>();
        enumConnections.Setup(o => o.EnumerateConnections()).Returns(new RasConnection[0]);

        services.Setup(o => o.GetService(typeof(IRasEnumConnections))).Returns(enumConnections.Object);
        var result = RasConnection.EnumerateConnections();

        Assert.IsNotNull(result);
        services.Verify(o => o.GetService(typeof(IRasEnumConnections)), Times.Once);
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

        services.Setup(o => o.GetService(typeof(IRasEnumConnections))).Returns(enumConnections.Object);

        var result = RasConnection.EnumerateConnections().SingleOrDefault(o => o.EntryName == "Test2");

        Assert.IsNotNull(result);
        Assert.AreSame(connection2.Object, result);
    }

    [Test]
    public void ReturnTheHandle()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(handle, target.Handle);
    }

    [Test]
    public void ReturnTheDevice()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(device, target.Device);
    }

    [Test]
    public void ReturnTheEntryName()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(entryName, target.EntryName);
    }

    [Test]
    public void ReturnThePhoneBook()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(phoneBook, target.PhoneBookPath);
    }

    [Test]
    public void ReturnTheEntryId()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(entryId, target.EntryId);
    }

    [Test]
    public void ReturnTheOptions()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(options, target.Options);
    }

    [Test]
    public void ReturnTheSessionId()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(sessionId, target.SessionId);
    }

    [Test]
    public void ReturnTheCorrelationId()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        Assert.AreEqual(correlationId, target.CorrelationId);
    }

    [Test]
    public void ConstructorThrowsExceptionWhenHandleIsNull()
    {
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(IntPtr.Zero, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void ConstructorThrowsExceptionWhenDeviceIsNull()
    {
        var handle = new IntPtr(1);
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(handle, null, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void ConstructorThrowsExceptionWhenEntryNameIsNull()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(handle, device, null, phoneBook, entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void ConstructorThrowsExceptionWhenEntryNameIsEmpty()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(handle, device, "", phoneBook, entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void ConstructorThrowsExceptionWhenEntryNameIsWhitespace()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(handle, device, "                ", phoneBook, entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void ConstructorThrowsExceptionWhenPhoneBookIsNull()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(handle, device, entryName, null, entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void ConstructorThrowsExceptionWhenPhoneBookIsEmpty()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(handle, device, entryName, "", entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void ConstructorThrowsExceptionWhenPhoneBookIsWhitespace()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => _ = new RasConnection(handle, device, entryName, "             ", entryId, options, sessionId, correlationId, services.Object));
    }

    [Test]
    public void RetrievesTheStatusAsExpected()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var status = new Mock<RasConnectionStatus>();
        var rasGetConnectStatus = new Mock<IRasGetConnectStatus>();

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);

        services.Setup(o => o.GetService(typeof(IRasGetConnectStatus))).Returns(rasGetConnectStatus.Object);
        rasGetConnectStatus.Setup(o => o.GetConnectionStatus(target)).Returns(status.Object).Verifiable();

        var result = target.GetStatus();

        Assert.AreEqual(status.Object, result);
        rasGetConnectStatus.Verify();
    }

    [Test]
    public void RetrievesTheStatisticsAsExpected()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasConnectionStatistics = new Mock<RasConnectionStatistics>();

        var rasGetConnectionStatistics = new Mock<IRasGetConnectionStatistics>();
        services.Setup(o => o.GetService(typeof(IRasGetConnectionStatistics))).Returns(rasGetConnectionStatistics.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);

        rasGetConnectionStatistics.Setup(o => o.GetConnectionStatistics(target)).Returns(rasConnectionStatistics.Object).Verifiable();

        var result = target.GetStatistics();

        Assert.AreEqual(rasConnectionStatistics.Object, result);
        rasGetConnectionStatistics.Verify();
    }

    [Test]
    public void DisconnectWithoutToken()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        target.Disconnect();

        rasHangUp.Verify(o => o.HangUpAsync(target, It.IsAny<bool>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task DisconnectAsyncWithoutToken()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        await target.DisconnectAsync();

        rasHangUp.Verify(o => o.HangUpAsync(target, It.IsAny<bool>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task DisconnectAsyncTheConnectionAsExpected()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var cancellationToken = CancellationToken.None;

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        await target.DisconnectAsync(CancellationToken.None);

        rasHangUp.Verify(o => o.HangUpAsync(target, It.IsAny<bool>(), cancellationToken), Times.Once);
    }

    [Test]
    public void DisconnectTheConnectionAsExpected()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var cancellationToken = CancellationToken.None;

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        target.Disconnect(CancellationToken.None);

        rasHangUp.Verify(o => o.HangUpAsync(target, It.IsAny<bool>(), cancellationToken));
    }

    [Test]
    public void ClearsTheConnectionStatisticsAsExpected()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasClearConnectionStatistics = new Mock<IRasClearConnectionStatistics>();
        services.Setup(o => o.GetService(typeof(IRasClearConnectionStatistics)))
            .Returns(rasClearConnectionStatistics.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        target.ClearStatistics();

        rasClearConnectionStatistics.Verify(o => o.ClearConnectionStatistics(target), Times.Once);
    }

    [Test]
    public void DisconnectShouldCloseAllReferencesByDefault()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        target.Disconnect(CancellationToken.None);

        rasHangUp.Verify(o => o.HangUpAsync(target, true, CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task DisconnectAsyncShouldCloseAllReferencesByDefault()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        await target.DisconnectAsync(CancellationToken.None);

        rasHangUp.Verify(o => o.HangUpAsync(target, true, CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task DisconnectAsyncShouldOptionallyCloseAllReferences()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        await target.DisconnectAsync(CancellationToken.None, false);

        rasHangUp.Verify(o => o.HangUpAsync(target, false, CancellationToken.None), Times.Once);
    }

    [Test]
    public void DisconnectShouldOptionallyCloseAllReferences()
    {
        var handle = new IntPtr(1);
        var device = new TestDevice("Test");
        var entryName = "Test";
        var phoneBook = @"C:\Test.pbk";
        var entryId = Guid.NewGuid();
        var options = new RasConnectionOptions(Ras.RASCF.AllUsers);
        var sessionId = new Luid(1, 1);
        var correlationId = Guid.NewGuid();

        var rasHangUp = new Mock<IRasHangUp>();
        services.Setup(o => o.GetService(typeof(IRasHangUp))).Returns(rasHangUp.Object);

        var target = new RasConnection(handle, device, entryName, phoneBook, entryId, options, sessionId, correlationId, services.Object);
        target.Disconnect(CancellationToken.None, false);

        rasHangUp.Verify(o => o.HangUpAsync(target, false, CancellationToken.None), Times.Once);
    }

}