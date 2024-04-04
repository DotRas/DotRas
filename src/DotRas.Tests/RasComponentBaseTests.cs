using System.ComponentModel;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests;

[TestFixture]
public class RasComponentBaseTests
{
    [Test]
    public void SwallowsErrorsWhichOccurWhileHandlingErrors()
    {
        var target = new StubRasComponent();
        target.Error += (sender, e) =>
        {
            throw new TestException();
        };

        Assert.DoesNotThrow(() => target.RaiseInternalErrorEvent(new ErrorEventArgs(new TestException())));
    }

    [Test]
    public void RaisesTheEventUsingTheSynchronizingObjectWhenRequired()
    {
        var synchronizingObject = new Mock<ISynchronizeInvoke>();
        synchronizingObject.Setup(o => o.InvokeRequired).Returns(true);

        var target = new StubRasComponent
        {
            SynchronizingObject = synchronizingObject.Object
        };

        target.SomethingOccurred += (sender, e) => { };
        target.RaiseSomethingOccurredEvent(EventArgs.Empty);

        synchronizingObject.Verify(o => o.Invoke(It.IsAny<Delegate>(), It.IsAny<object[]>()), Times.Once);
    }

    [Test]
    public void RaisesTheEventWithoutUsingTheSynchronizingObjectWhenNotRequired()
    {
        var synchronizingObject = new Mock<ISynchronizeInvoke>();
        synchronizingObject.Setup(o => o.InvokeRequired).Returns(false);

        var target = new StubRasComponent
        {
            SynchronizingObject = synchronizingObject.Object
        };

        target.SomethingOccurred += (sender, e) => { };
        target.RaiseSomethingOccurredEvent(EventArgs.Empty);

        synchronizingObject.Verify(o => o.Invoke(It.IsAny<Delegate>(), It.IsAny<object[]>()), Times.Never);
    }
}