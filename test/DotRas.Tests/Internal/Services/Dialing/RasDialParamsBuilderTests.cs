using System;
using System.Net;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.Dialing;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services.Dialing
{
    [TestFixture]
    public class RasDialParamsBuilderTests
    {
        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasDialParamsBuilder(null, new Mock<IRasGetCredentials>().Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenRasGetCredentialsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasDialParamsBuilder(new Mock<IStructFactory>().Object, null));
        }

        [Test]
        public void BuildsTheStructureWithTheStoredCredentialsWithNullCredentials()
        {
            var entryName = "Test";
            var phoneBookPath = @"C:\Test.pbk";

            var structFactory = new Mock<IStructFactory>();

            var credentials = new NetworkCredential("User", "Password", "Domain");
            var rasGetCredentials = new Mock<IRasGetCredentials>();
            rasGetCredentials.Setup(o => o.GetNetworkCredential(entryName, phoneBookPath)).Returns(credentials);

            var context = new RasDialContext
            {
                EntryName = entryName,
                PhoneBookPath = phoneBookPath,
                Credentials = null,
                Options = new RasDialerOptions
                {
                    AllowUseStoredCredentials = true
                }
            };

            var target = new RasDialParamsBuilder(structFactory.Object, rasGetCredentials.Object);
            var result = target.Build(context);

            Assert.AreEqual("User", result.szUserName);
            Assert.AreEqual("Password", result.szPassword);
            Assert.AreEqual("Domain", result.szDomain);
        }

        [Test]
        public void BuildsTheStructureWithTheStoredCredentialsWithNullUserName()
        {
            var entryName = "Test";
            var phoneBookPath = @"C:\Test.pbk";

            var structFactory = new Mock<IStructFactory>();

            var credentials = new NetworkCredential("User", "Password", "Domain");
            var rasGetCredentials = new Mock<IRasGetCredentials>();
            rasGetCredentials.Setup(o => o.GetNetworkCredential(entryName, phoneBookPath)).Returns(credentials);

            var context = new RasDialContext
            {
                EntryName = entryName,
                PhoneBookPath = phoneBookPath,
                Credentials = null,
                Options = new RasDialerOptions
                {
                    AllowUseStoredCredentials = true
                }
            };

            var target = new RasDialParamsBuilder(structFactory.Object, rasGetCredentials.Object);
            var result = target.Build(context);

            Assert.AreEqual("User", result.szUserName);
            Assert.AreEqual("Password", result.szPassword);
            Assert.AreEqual("Domain", result.szDomain);
        }

        [Test]
        public void BuildsTheStructureWithTheEntryName()
        {
            var structFactory = new Mock<IStructFactory>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var context = new RasDialContext
            {
                EntryName = "Test"
            };

            var target = new RasDialParamsBuilder(structFactory.Object, rasGetCredentials.Object);
            var result = target.Build(context);

            Assert.AreEqual("Test", result.szEntryName);
        }

        [Test]
        public void BuildsTheStructureWithTheSubEntryId()
        {
            var structFactory = new Mock<IStructFactory>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var context = new RasDialContext
            {
                Options = new RasDialerOptions
                {
                    SubEntryId = 1
                }
            };

            var target = new RasDialParamsBuilder(structFactory.Object, rasGetCredentials.Object);
            var result = target.Build(context);

            Assert.AreEqual(1, result.dwSubEntry);
        }

        [Test]
        public void BuildsTheStructureWithTheInterfaceIndex()
        {
            var structFactory = new Mock<IStructFactory>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var context = new RasDialContext
            {
                Options = new RasDialerOptions
                {
                    InterfaceIndex = 1
                }
            };

            var target = new RasDialParamsBuilder(structFactory.Object, rasGetCredentials.Object);
            var result = target.Build(context);

            Assert.AreEqual(1, result.dwIfIndex);
        }

        [Test]
        public void BuildsTheStructureWithTheUserNameAndPassword()
        {
            var structFactory = new Mock<IStructFactory>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var context = new RasDialContext
            {
                Credentials = new NetworkCredential("User", "Pass")
            };

            var target = new RasDialParamsBuilder(structFactory.Object, rasGetCredentials.Object);
            var result = target.Build(context);

            Assert.AreEqual("User", result.szUserName);
            Assert.AreEqual("Pass", result.szPassword);
        }

        [Test]
        public void BuildsTheStructureWithTheUserNamePasswordAndDomain()
        {
            var structFactory = new Mock<IStructFactory>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var context = new RasDialContext
            {
                Credentials = new NetworkCredential("User", "Pass", "Domain")
            };

            var target = new RasDialParamsBuilder(structFactory.Object, rasGetCredentials.Object);
            var result = target.Build(context);

            Assert.AreEqual("User", result.szUserName);
            Assert.AreEqual("Pass", result.szPassword);
            Assert.AreEqual("Domain", result.szDomain);
        }
    }
}