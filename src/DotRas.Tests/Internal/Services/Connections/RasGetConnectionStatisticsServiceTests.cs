﻿using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Connections;

[TestFixture]
public class RasGetConnectionStatisticsServiceTests
{
    private delegate int RasGetConnectionStatisticsCallback(
        IntPtr handle,
        ref RAS_STATS statistics);

    [Test]
    public void ThrowsAnExceptionWhenApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasGetConnectionStatisticsService(null, new Mock<IStructFactory>().Object, new Mock<IExceptionPolicy>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenStructFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasGetConnectionStatisticsService(new Mock<IRasApi32>().Object, null, new Mock<IExceptionPolicy>().Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasGetConnectionStatisticsService(new Mock<IRasApi32>().Object, new Mock<IStructFactory>().Object, null));
    }

    [Test]
    public void ThrowsAnExceptionWhenHandleIsNull()
    {
        var api = new Mock<IRasApi32>();
        var structFactory = new Mock<IStructFactory>();
        var exceptionPolicy = new Mock<IExceptionPolicy>();

        var target = new RasGetConnectionStatisticsService(api.Object, structFactory.Object, exceptionPolicy.Object);
        Assert.Throws<ArgumentNullException>(() => target.GetConnectionStatistics(null));
    }

    [Test]
    public void ReturnsTheStatisticsAsExpected()
    {
        var handle = new IntPtr(1);
        var api = new Mock<IRasApi32>();
        api.Setup(o => o.RasGetConnectionStatistics(handle, ref It.Ref<RAS_STATS>.IsAny)).Returns(new RasGetConnectionStatisticsCallback(
            (IntPtr h, ref RAS_STATS stats) =>
            {
                stats.dwBytesXmited = 1;
                stats.dwBytesRcved = 2;
                stats.dwFramesXmited = 3;
                stats.dwFramesRcved = 4;
                stats.dwCrcErr = 5;
                stats.dwTimeoutErr = 6;
                stats.dwAlignmentErr = 7;
                stats.dwHardwareOverrunErr = 8;
                stats.dwFramingErr = 9;
                stats.dwBufferOverrunErr = 10;
                stats.dwCompressionRatioIn = 11;
                stats.dwCompressionRatioOut = 12;
                stats.dwBps = 13;
                stats.dwConnectDuration = 14;

                return SUCCESS;
            }));

        var connection = new Mock<IRasConnection>();
        connection.Setup(o => o.Handle).Returns(handle);

        var structFactory = new Mock<IStructFactory>();
        structFactory.Setup(o => o.Create<RAS_STATS>()).Returns(new RAS_STATS());

        var exceptionPolicy = new Mock<IExceptionPolicy>();

        var target = new RasGetConnectionStatisticsService(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.GetConnectionStatistics(connection.Object);

        Assert.AreEqual(1, result.BytesTransmitted);
        Assert.AreEqual(2, result.BytesReceived);
        Assert.AreEqual(3, result.FramesTransmitted);
        Assert.AreEqual(4, result.FramesReceived);
        Assert.AreEqual(5, result.CrcErrors);
        Assert.AreEqual(6, result.TimeoutErrors);
        Assert.AreEqual(7, result.AlignmentErrors);
        Assert.AreEqual(8, result.HardwareOverrunErrors);
        Assert.AreEqual(9, result.FramingErrors);
        Assert.AreEqual(10, result.BufferOverrunErrors);
        Assert.AreEqual(11, result.CompressionRatioIn);
        Assert.AreEqual(12, result.CompressionRatioOut);
        Assert.AreEqual(13, result.LinkSpeed);
        Assert.AreEqual(14, result.ConnectionDuration.TotalMilliseconds);
    }

    [Test]
    public void ThrowsAnExceptionWhenTheApiResultIsNonZero()
    {
        var handle = new IntPtr(1);
        var api = new Mock<IRasApi32>();
        api.Setup(o => o.RasGetConnectionStatistics(handle, ref It.Ref<RAS_STATS>.IsAny)).Returns(ERROR_INVALID_PARAMETER);

        var structFactory = new Mock<IStructFactory>();
        structFactory.Setup(o => o.Create<RAS_STATS>()).Returns(new RAS_STATS());

        var connection = new Mock<IRasConnection>();
        connection.Setup(o => o.Handle).Returns(handle);

        var exceptionPolicy = new Mock<IExceptionPolicy>();
        exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

        var target = new RasGetConnectionStatisticsService(api.Object, structFactory.Object, exceptionPolicy.Object);
        Assert.Throws<TestException>(() => target.GetConnectionStatistics(connection.Object));
    }
}