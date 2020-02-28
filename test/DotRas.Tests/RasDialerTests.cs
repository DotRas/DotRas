using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Services;
using DotRas.Tests.Stubs;
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
            Assert.DoesNotThrow(() => new RasDialer());
        }

        [Test]
        public void RaisesTheErrorEventWhenAnErrorOccursDuringStateChanged()
        {
            bool called = false;

            var api = new Mock<IRasDial>();

            var target = new TestableRasDialer(api.Object);
            target.StateChanged += (sender, e) =>
            {
                throw new TestException();
            };

            target.Error += (sender, e) =>
            {
                Assert.IsInstanceOf<TestException>(e.GetException());
                called = true;
            };

            target.RaiseStateChangedEvent(new StateChangedEventArgs(RasConnectionState.AuthNotify));

            Assert.True(called, "The event was not called as expected.");
        }

        [Test]
        public void ConnectSynchronouslyWithAnExistingCancellationToken()
        {
            var connection = new Mock<RasConnection>();
            var cancellationToken = new CancellationToken();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(cancellationToken, c.CancellationToken);

                return Task.FromResult(connection.Object);
            }).Verifiable();

            var target = new RasDialer(api.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            var result = target.Connect(cancellationToken);
            
            Assert.IsNotNull(result);
            api.Verify();
        }

        [Test]
        public void ConnectSynchronouslyWithoutACancellationToken()
        {
            var connection = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(CancellationToken.None, c.CancellationToken);
                
                return Task.FromResult(connection.Object);
            }).Verifiable();

            var target = new RasDialer(api.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            var result = target.Connect();

            Assert.IsNotNull(result);
            api.Verify();
        }

        [Test]
        public async Task ConnectAsynchronouslyWithoutACancellationToken()
        {
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>())).Returns<RasDialContext>(c =>
            {
                Assert.AreEqual(CancellationToken.None, c.CancellationToken);

                return Task.FromResult(result.Object);
            }).Verifiable();

            var target = new RasDialer(api.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            await target.ConnectAsync();

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

            var target = new RasDialer(api.Object)
            {
                Credentials = credentials,
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            var connection = await target.ConnectAsync(cancellationToken);
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

            var target = new RasDialer(api.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            await target.ConnectAsync();

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

            var raised = false;

            var target = new RasDialer(api.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            target.StateChanged += (sender, args) =>
            {
                Assert.AreEqual(e, args);
                raised = true;
            };

            await target.ConnectAsync();

            Assert.True(raised);
        }

        [Test]
        public void DisposesTheApiAsExpected()
        {
            var api = new Mock<IRasDial>();

            var target = new RasDialer(api.Object);
            target.Dispose();

            api.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public void DoesNotThrowsAnExceptionWhenThePhoneBookPathHasNotBeenSet()
        {
            var api = new Mock<IRasDial>();

            var target = new RasDialer(api.Object)
            {
                EntryName = EntryName,
                PhoneBookPath = null
            };

            Assert.DoesNotThrow(() => target.Connect());
        }

        [Test]
        public void IndicatesTheObjectIsBusyAsExpected()
        {
            var api = new Mock<IRasDial>();
            api.Setup(o => o.IsBusy).Returns(true);

            var target = new RasDialer(api.Object);
            Assert.True(target.IsBusy);
        }

        [Test]
        public void IndicatesTheObjectIsNotBusyAsExpected()
        {
            var api = new Mock<IRasDial>();
            api.Setup(o => o.IsBusy).Returns(false);

            var target = new RasDialer(api.Object);
            Assert.False(target.IsBusy);
        }
    }
}