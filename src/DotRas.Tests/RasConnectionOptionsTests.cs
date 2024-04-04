using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests;

[TestFixture]
public class RasConnectionOptionsTests
{
    [Test]
    public void MustReturnTrueForAllUsers()
    {
        var target = new RasConnectionOptions(RASCF.AllUsers);

        Assert.True(target.AvailableToAllUsers);
        Assert.False(target.UsingDefaultCredentials);
        Assert.False(target.IsOwnerKnown);
        Assert.False(target.IsOwnerCurrentUser);
    }

    [Test]
    public void MustReturnTrueForGlobalCreds()
    {
        var target = new RasConnectionOptions(RASCF.GlobalCreds);

        Assert.False(target.AvailableToAllUsers);
        Assert.True(target.UsingDefaultCredentials);
        Assert.False(target.IsOwnerKnown);
        Assert.False(target.IsOwnerCurrentUser);
    }

    [Test]
    public void MustReturnTrueForOwnerKnown()
    {
        var target = new RasConnectionOptions(RASCF.OwnerKnown);

        Assert.False(target.AvailableToAllUsers);
        Assert.False(target.UsingDefaultCredentials);
        Assert.True(target.IsOwnerKnown);
        Assert.False(target.IsOwnerCurrentUser);
    }

    [Test]
    public void MustReturnTrueForOwnerMatch()
    {
        var target = new RasConnectionOptions(RASCF.OwnerMatch);

        Assert.False(target.AvailableToAllUsers);
        Assert.False(target.UsingDefaultCredentials);
        Assert.False(target.IsOwnerKnown);
        Assert.True(target.IsOwnerCurrentUser);
    }
}