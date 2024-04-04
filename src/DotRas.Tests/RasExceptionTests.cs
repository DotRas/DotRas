using NUnit.Framework;

namespace DotRas.Tests;

[TestFixture]
public class RasExceptionTests
{
    [Test]
    public void InitializeTheExceptionWithAMessage()
    {
        var target = new RasException(623, "This is a test exception!");

        Assert.AreEqual(623, target.NativeErrorCode);
        Assert.AreEqual("This is a test exception!", target.Message);
    }
}