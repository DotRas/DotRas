using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.Dialing;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services.Dialing;

[TestFixture]
public class DefaultRasDialCallbackHandlerTests
{
    private Mock<IRasHangUp> rasHangUp;
    private Mock<IRasEnumConnections> rasEnumConnections;
    private Mock<IExceptionPolicy> exceptionPolicy;
    private Mock<IValueWaiter<IntPtr>> waitHandle;
    private TaskCompletionSource<RasConnection> completionSource;

    [SetUp]
    public void Init()
    {
        rasHangUp = new Mock<IRasHangUp>();
        rasEnumConnections = new Mock<IRasEnumConnections>();
        exceptionPolicy = new Mock<IExceptionPolicy>();
        waitHandle = new Mock<IValueWaiter<IntPtr>>();
        completionSource = new TaskCompletionSource<RasConnection>();
    }

    [Test]
    public void ThrowsAnExceptionWhenTheHangUpApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new DefaultRasDialCallbackHandler(null, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        });
    }

    [Test]
    public void ThrowsAnExceptionWhenTheRasEnumConnectionsApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new DefaultRasDialCallbackHandler(rasHangUp.Object, null, exceptionPolicy.Object, waitHandle.Object);
        });
    }

    [Test]
    public void ThrowsAnExceptionWhenExceptionPolicyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, null, waitHandle.Object);
        });
    }

    [Test]
    public void ThrowsAnExceptionWhenHandleIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, null);
        });
    }

    [Test]
    public void ThrowsAnExceptionWhenTheCompletionSourceIsNull()
    {
        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        Assert.Throws<ArgumentNullException>(() => target.Initialize(null, e => { }, () => { }, CancellationToken.None));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheOnStateChangedCallbackIsNull()
    {
        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        Assert.Throws<ArgumentNullException>(() => target.Initialize(completionSource, null, () => { }, CancellationToken.None));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheOnCompletedCallbackIsNull()
    {
        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        Assert.Throws<ArgumentNullException>(() => target.Initialize(completionSource, e => { }, null, CancellationToken.None));
    }


    [Test]
    public void MustSupportMultipleInitializeForReuseOfHandler()
    {
        var cancellationToken = new CancellationToken(true);

        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);

        target.Initialize(completionSource, (e) => { }, () => { }, cancellationToken);
        Assert.IsTrue(target.Initialized);

        target.Initialize(completionSource, (e) => { }, () => { }, cancellationToken);
        Assert.IsTrue(target.Initialized);
    }

    [Test]
    public void RaisesTheCallbackAction()
    {
        var handle = new IntPtr(1);
        waitHandle.Setup(o => o.Value).Returns(handle);

        var called = false;

        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        target.Initialize(completionSource, (e) =>
        {
            Assert.AreEqual(RasConnectionState.OpenPort, e.State);
            called = true;
        }, () => { }, CancellationToken.None);

        var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.OpenPort, 0, 0);

        Assert.IsTrue(result);
        Assert.IsTrue(called);
    }

    [Test]
    public void ThrowsAnExceptionWhenTheDwErrorCodeIsNonZero()
    {
        var handle = new IntPtr(1);
        exceptionPolicy.Setup(o => o.Create(632)).Returns(new Exception("An exception occurred!")).Verifiable();

        waitHandle.Setup(o => o.Value).Returns(handle);

        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        target.Initialize(completionSource, (e) => { }, () => { }, CancellationToken.None);

        var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.OpenPort, 632, 0);

        Assert.IsFalse(result);
        Assert.IsTrue(target.HasEncounteredErrors);
    }

    [Test]
    public void ThrowsAnExceptionWhenOnCallbackIsNotInitialized()
    {
        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        Assert.Throws<InvalidOperationException>(() => target.OnCallback(new IntPtr(1), 1, new IntPtr(1), 0, RasConnectionState.OpenPort, 0, 0));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheHandleIsNull()
    {
        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        Assert.Throws<ArgumentNullException>(() => target.SetHandle(IntPtr.Zero));
    }

    [Test]
    public void SetsTheHandle()
    {
        var handle = new IntPtr(1);

        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        target.SetHandle(handle);

        waitHandle.Verify(o => o.Set(handle), Times.Once);
    }

    [Test]
    public void ThrowsAnExceptionWhenTheHandleIsAlreadySet()
    {
        var handle = new IntPtr(1);
        waitHandle.Setup(o => o.IsSet).Returns(true);

        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        Assert.Throws<InvalidOperationException>(() => target.SetHandle(handle));
    }

    [Test]
    public void ReturnsAnExceptionWhenTheFactoryDoesNotReturnAConnection()
    {
        var handle = new IntPtr(1);
        waitHandle.Setup(o => o.Value).Returns(handle);

        var target = new StubDefaultRasDialCallbackHandler(_ => null, rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        target.Initialize(completionSource, (e) => { }, () => { }, CancellationToken.None);

        var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.Connected, 0, 0);

        Assert.IsFalse(result);
        Assert.IsTrue(target.HasEncounteredErrors);
    }

    [Test]
    public void ReturnsTheConnectionWhenConnected()
    {
        var handle = new IntPtr(1);

        var connection = new Mock<RasConnection>();
        connection.Setup(o => o.Handle).Returns(handle);

        rasEnumConnections.Setup(o => o.EnumerateConnections()).Returns(new[]
        {
            connection.Object
        });

        waitHandle.Setup(o => o.Value).Returns(handle);

        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        target.Initialize(completionSource, (e) => { }, () => { }, CancellationToken.None);

        var result = target.OnCallback(new IntPtr(1), 0, new IntPtr(1), 0, RasConnectionState.Connected, 0, 0);

        Assert.IsFalse(result);
        Assert.IsFalse(target.HasEncounteredErrors);
        Assert.IsTrue(target.Completed);
    }

    [Test]
    public void DisposesTheHandleDuringDispose()
    {
        var target = new DefaultRasDialCallbackHandler(rasHangUp.Object, rasEnumConnections.Object, exceptionPolicy.Object, waitHandle.Object);
        target.Dispose();

        waitHandle.Verify(o => o.Dispose(), Times.Once);
    }
}