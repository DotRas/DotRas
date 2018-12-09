using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Providers;
using DotRas.Internal.Abstractions.Services;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasDialerTests
    {
        private const string PhoneBookPath = "PATH";
        private const string EntryName = "ENTRY";

        [Test]
        public void CanInstantiateTheDialer()
        {
            var target = new RasDialer();
            Assert.IsNotNull(target);
        }

        [Test]
        public void DialTheConnectionSynchronouslyWithAnExistingCancellationToken()
        {
            var connection = new Mock<RasConnection>();
            var cancellationToken = new CancellationToken();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(cancellationToken, c.CancellationToken);

                return Task.FromResult(connection.Object);
            }).Verifiable();

            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(true);

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            var result = target.Dial(cancellationToken);

            Assert.IsNotNull(result);
            api.Verify();
        }

        [Test]
        public void DialsTheConnectionSynchronouslyWithoutACancellationToken()
        {
            var connection = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(CancellationToken.None, c.CancellationToken);
                
                return Task.FromResult(connection.Object);
            }).Verifiable();

            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(true);

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            var result = target.Dial();

            Assert.IsNotNull(result);
            api.Verify();
        }

        [Test]
        public async Task DialsTheConnectionAsyncWithoutACancellationToken()
        {
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(CancellationToken.None, c.CancellationToken);

                return Task.FromResult(result.Object);
            }).Verifiable();

            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(true);

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            await target.DialAsync();

            api.Verify();
        }

        [Test]
        public async Task BuildsTheContextCorrectly()
        {
            var cancellationToken = CancellationToken.None;
            var credentials = new NetworkCredential("USERNAME", "PASSWORD", "DOMAIN");
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(cancellationToken, c.CancellationToken);
                Assert.AreEqual(credentials, c.Credentials);
                Assert.AreEqual(EntryName, c.EntryName);
                Assert.AreEqual(PhoneBookPath, c.PhoneBookPath);

                return Task.FromResult(result.Object);
            });

            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(true);

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                Credentials = credentials,
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            var connection = await target.DialAsync(cancellationToken);
            Assert.AreSame(result.Object, connection);
        }

        [Test]
        public async Task BuildsTheContextWithTheStoredCredentialsAsExpected()
        {
            var cancellationToken = CancellationToken.None;
            var credentials = new NetworkCredential("USERNAME", "PASSWORD", "DOMAIN");
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(cancellationToken, c.CancellationToken);
                Assert.AreEqual(credentials, c.Credentials);
                Assert.AreEqual(EntryName, c.EntryName);
                Assert.AreEqual(PhoneBookPath, c.PhoneBookPath);

                return Task.FromResult(result.Object);
            });

            var rasGetCredentials = new Mock<IRasGetCredentials>();
            rasGetCredentials.Setup(o => o.GetNetworkCredential(EntryName, PhoneBookPath)).Returns(credentials);

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(true);

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                AllowUseStoredCredentials = true
            };

            var connection = await target.DialAsync(cancellationToken);
            Assert.AreSame(result.Object, connection);
        }

        [Test]
        public async Task ThrowsAnExceptionWhenTheEventArgsIsNull()
        {
            var executed = false;
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.IsNotNull(c.OnStateChangedCallback);
                Assert.Throws<ArgumentNullException>(() => c.OnStateChangedCallback(null));

                executed = true;
                return Task.FromResult(result.Object);
            });

            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(true);

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            await target.DialAsync();

            Assert.True(executed);
        }

        [Test]
        public async Task RaisesTheEventFromTheOnStateChangedCallback()
        {
            var e = new StateChangedEventArgs(RasConnectionState.OpenPort);
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.IsNotNull(c.OnStateChangedCallback);
                c.OnStateChangedCallback(e);                

                return Task.FromResult(result.Object);
            });

            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(true);

            var raised = false;

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            target.StateChanged += (sender, args) =>
            {
                Assert.AreEqual(e, args);
                raised = true;
            };

            await target.DialAsync();

            Assert.True(raised);
        }

        [Test]
        public void DisposesTheApiAsExpected()
        {
            var api = new Mock<IRasDial>();
            var disposable = api.As<IDisposable>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            var validator = new Mock<IPhoneBookEntryValidator>();

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object);
            target.Dispose();

            disposable.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheEntryNameHasNotBeenSet()
        {
            var api = new Mock<IRasDial>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();

            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = null,
                PhoneBookPath = PhoneBookPath
            };

            Assert.Throws<RasDialerConfigurationException>(() => target.Dial());
        }

        [Test]
        public void DoesNotThrowsAnExceptionWhenThePhoneBookPathHasNotBeenSet()
        {
            var api = new Mock<IRasDial>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();
            var fileSystem = new Mock<IFileSystem>();

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, null)).Returns(true);

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = null
            };

            Assert.DoesNotThrow(() => target.Dial());
        }

        [Test]
        public void ThrowsAnExceptionWhenThePhoneBookPathDoesNotExist()
        {
            var api = new Mock<IRasDial>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(false).Verifiable();

            var validator = new Mock<IPhoneBookEntryValidator>();

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            Assert.Throws<FileNotFoundException>(() => target.Dial());

            fileSystem.Verify();
        }

        [Test]
        public void ThrowsAnExceptionWhenTheEntryNameDoesNotExist()
        {
            var api = new Mock<IRasDial>();
            var rasGetCredentials = new Mock<IRasGetCredentials>();
            var fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup(o => o.VerifyFileExists(PhoneBookPath)).Returns(true);

            var validator = new Mock<IPhoneBookEntryValidator>();
            validator.Setup(o => o.VerifyEntryExists(EntryName, PhoneBookPath)).Returns(false).Verifiable();

            var target = new RasDialer(api.Object, rasGetCredentials.Object, fileSystem.Object, validator.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            Assert.Throws<RasDialerConfigurationException>(() => target.Dial());

            validator.Verify();
        }
    }
}