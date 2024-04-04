﻿using System.Net;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Dialing;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Dialing;

[TestFixture]
public class RasDialParamsBuilderTests
{
    private delegate int RasGetEntryDialParamsCallback(
        string phoneBookPath,
        ref RASDIALPARAMS dialParams,
        out bool foundPassword);

    private Mock<IRasApi32> api;
    private Mock<IStructFactory> structFactory;
    private Mock<IExceptionPolicy> exceptionPolicy;

    [SetUp]
    public void Init()
    {
        api = new Mock<IRasApi32>();
        structFactory = new Mock<IStructFactory>();
        exceptionPolicy = new Mock<IExceptionPolicy>();
    }

    [Test]
    public void ThrowsAnExceptionWhenApiIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasDialParamsBuilder(null, structFactory.Object, exceptionPolicy.Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenStructFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasDialParamsBuilder(api.Object, null, exceptionPolicy.Object));
    }

    [Test]
    public void ThrowsAnExceptionWhenRasGetCredentialsIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RasDialParamsBuilder(api.Object, structFactory.Object, null));
    }

    [Test]
    public void ThrowsAnExceptionWhenContextIsNull()
    {
        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        Assert.Throws<ArgumentNullException>(() => target.Build(null));
    }

    [Test]
    public void ThrowsAnExceptionWhenTheApiResultIsNonZero()
    {
        var entryName = "Test";
        var phoneBookPath = @"C:\Test.pbk";

        api.Setup(o => o.RasGetEntryDialParams(phoneBookPath, ref It.Ref<RASDIALPARAMS>.IsAny, out It.Ref<bool>.IsAny)).Returns(ERROR_INSUFFICIENT_BUFFER);

        exceptionPolicy.Setup(o => o.Create(ERROR_INSUFFICIENT_BUFFER)).Returns(new TestException());

        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        Assert.Throws<TestException>(() => target.Build(new RasDialContext
        {
            EntryName = entryName,
            PhoneBookPath = phoneBookPath
        }));
    }

    [Test]
    public void BuildsTheStructureWithTheStoredCredentialsWithNullCredentials()
    {
        var entryName = "Test";
        var phoneBookPath = @"C:\Test.pbk";

        api.Setup(o => o.RasGetEntryDialParams(phoneBookPath, ref It.Ref<RASDIALPARAMS>.IsAny, out It.Ref<bool>.IsAny)).Returns(new RasGetEntryDialParamsCallback(
            (string o1, ref RASDIALPARAMS o2, out bool o3) =>
            {
                o2.szUserName = "User";
                o2.szPassword = "Password";
                o2.szDomain = "Domain";

                o3 = true;
                return SUCCESS;
            }));

        var context = new RasDialContext
        {
            EntryName = entryName,
            PhoneBookPath = phoneBookPath,
            Credentials = null
        };

        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.Build(context);

        Assert.AreEqual("User", result.szUserName);
        Assert.AreEqual("Password", result.szPassword);
        Assert.AreEqual("Domain", result.szDomain);
    }

    [Test]
    public void OverwritesTheStoredCredentialsWhenCredentialsAreSupplied()
    {
        var entryName = "Test";
        var phoneBookPath = @"C:\Test.pbk";

        api.Setup(o => o.RasGetEntryDialParams(phoneBookPath, ref It.Ref<RASDIALPARAMS>.IsAny, out It.Ref<bool>.IsAny)).Returns(new RasGetEntryDialParamsCallback(
            (string o1, ref RASDIALPARAMS o2, out bool o3) =>
            {
                o2.szUserName = "User1";
                o2.szPassword = "Password1";
                o2.szDomain = "Domain1";

                o3 = true;
                return SUCCESS;
            }));

        var context = new RasDialContext
        {
            EntryName = entryName,
            PhoneBookPath = phoneBookPath,
            Credentials = new NetworkCredential("User2", "Password2", "Domain2")
        };

        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.Build(context);

        Assert.AreEqual("User2", result.szUserName);
        Assert.AreEqual("Password2", result.szPassword);
        Assert.AreEqual("Domain2", result.szDomain);
    }

    [Test]
    public void BuildsTheStructureWithTheEntryName()
    {
        var context = new RasDialContext
        {
            EntryName = "Test"
        };

        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.Build(context);

        Assert.AreEqual("Test", result.szEntryName);
    }

    [Test]
    public void BuildsTheStructureWithTheInterfaceIndex()
    {
        var context = new RasDialContext
        {
            Options = new RasDialerOptions
            {
                InterfaceIndex = 1
            }
        };

        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.Build(context);

        Assert.AreEqual(1, result.dwIfIndex);
    }

    [Test]
    public void BuildsTheStructureWithTheUserNameAndPassword()
    {
        var context = new RasDialContext
        {
            Credentials = new NetworkCredential("User", "Pass")
        };

        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.Build(context);

        Assert.AreEqual("User", result.szUserName);
        Assert.AreEqual("Pass", result.szPassword);
    }

    [Test]
    public void BuildsTheStructureWithTheUserNamePasswordAndDomain()
    {
        var context = new RasDialContext
        {
            Credentials = new NetworkCredential("User", "Pass", "Domain")
        };

        var target = new RasDialParamsBuilder(api.Object, structFactory.Object, exceptionPolicy.Object);
        var result = target.Build(context);

        Assert.AreEqual("User", result.szUserName);
        Assert.AreEqual("Pass", result.szPassword);
        Assert.AreEqual("Domain", result.szDomain);
    }
}