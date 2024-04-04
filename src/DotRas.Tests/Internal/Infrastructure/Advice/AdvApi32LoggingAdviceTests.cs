using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Internal.Infrastructure.Advice;
using DotRas.Internal.Interop;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Infrastructure.Advice;

[TestFixture]
public class AdvApi32LoggingAdviceTests
{
    private delegate void LogEventCallback(EventLevel eventLevel, TraceEvent eventData);

    private Mock<IAdvApi32> api;
    private Mock<IEventLoggingPolicy> eventLoggingPolicy;

    [SetUp]
    public void Setup()
    {
        api = new Mock<IAdvApi32>();
        eventLoggingPolicy = new Mock<IEventLoggingPolicy>();
    }

    private delegate bool AllocateLocallyUniqueIdCallback(out Luid luid);

    [Test]
    public void AllocateLocallyUniqueIdAsExpected()
    {
        api.Setup(o => o.AllocateLocallyUniqueId(out It.Ref<Luid>.IsAny)).Returns(new AllocateLocallyUniqueIdCallback(
            (out Luid o1) =>
            {
                o1 = new Luid(10, 10);
                return true;
            }));

        eventLoggingPolicy.Setup(o => o.LogEvent(It.IsAny<EventLevel>(), It.IsAny<PInvokeBoolCallCompletedTraceEvent>())).Callback(new LogEventCallback(
            (level, o1) =>
            {
                Assert.AreEqual(EventLevel.Verbose, level);

                var eventData = (PInvokeBoolCallCompletedTraceEvent)o1;
                Assert.True(eventData.OutArgs.ContainsKey("luid"));
                Assert.True(eventData.Duration > TimeSpan.Zero);
                Assert.True(eventData.Result);
            })).Verifiable();

        var target = new AdvApi32LoggingAdvice(api.Object, eventLoggingPolicy.Object);
        var result = target.AllocateLocallyUniqueId(out var luid);

        Assert.True(result);
    }
}