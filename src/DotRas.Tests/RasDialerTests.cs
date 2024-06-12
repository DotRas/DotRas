using DotRas.Internal.Abstractions.Services;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DotRas.Tests {
    [TestFixture]
    public class RasDialerTests {
        private const string PhoneBookPath = "PATH";
        private const string EntryName = "ENTRY";

        [Test]
        public void CanInstantiateTheDialer() => Assert.DoesNotThrow(() => _ = new RasDialer());

        [Test]
        public void RaisesTheErrorEventWhenAnErrorOccursDuringStateChanged() {
            bool called = false;

            var api = new Mock<IRasDial>();

            var target = new TestableRasDialer(api.Object);
            target.StateChanged += (sender, e) => {
                throw new TestException();
            };

            target.Error += (sender, e) => {
                Assert.That(e.GetException(), Is.InstanceOf<TestException>());
                called = true;
            };

            target.RaiseStateChangedEvent(new StateChangedEventArgs(RasConnectionState.AuthNotify));

            Assert.That(called, Is.True, "The event was not called as expected.");
        }

        [Test]
        public void ConnectSynchronouslyWithAnExistingCancellationToken() {
            var connection = new Mock<RasConnection>();
            var cancellationToken = new CancellationToken();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>()))
                .Returns<RasDialContext>(c => {
                    Assert.That(c.CancellationToken, Is.EqualTo(cancellationToken));

                    return Task.FromResult(connection.Object);
                })
                .Verifiable();

            var target = new RasDialer(api.Object) {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            var result = target.Connect(cancellationToken);

            Assert.That(result, Is.Not.Null);
            api.Verify();
        }

        [Test]
        public void ConnectSynchronouslyWithoutACancellationToken() {
            var connection = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>()))
                .Returns<RasDialContext>(c => {
                    Assert.That(c.CancellationToken, Is.EqualTo(CancellationToken.None));

                    return Task.FromResult(connection.Object);
                })
                .Verifiable();

            var target = new RasDialer(api.Object) {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            var result = target.Connect();

            Assert.That(result, Is.Not.Null);
            api.Verify();
        }

        [Test]
        public async Task ConnectAsynchronouslyWithoutACancellationToken() {
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>()))
                .Returns<RasDialContext>(c => {
                    Assert.That(c.CancellationToken, Is.EqualTo(CancellationToken.None));

                    return Task.FromResult(result.Object);
                })
                .Verifiable();

            var target = new RasDialer(api.Object) {
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath,
                Credentials = new NetworkCredential("TEST", "USER")
            };

            await target.ConnectAsync();

            api.Verify();
        }

        [Test]
        public async Task BuildsTheContextCorrectly() {
            var cancellationToken = CancellationToken.None;
            var credentials = new NetworkCredential("USERNAME", "PASSWORD", "DOMAIN");
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>()))
                .Returns<RasDialContext>(c => {
                    Assert.Multiple(() => {
                        Assert.That(c.CancellationToken, Is.EqualTo(cancellationToken));
                        Assert.That(c.Credentials, Is.EqualTo(credentials));
                        Assert.That(c.EntryName, Is.EqualTo(EntryName));
                        Assert.That(c.PhoneBookPath, Is.EqualTo(PhoneBookPath));
                    });

                    return Task.FromResult(result.Object);
                });

            var target = new RasDialer(api.Object) {
                Credentials = credentials,
                EntryName = EntryName,
                PhoneBookPath = PhoneBookPath
            };

            var connection = await target.ConnectAsync(cancellationToken);
            Assert.That(connection, Is.SameAs(result.Object));
        }

        [Test]
        public async Task ThrowsAnExceptionWhenTheEventArgsIsNull() {
            var executed = false;
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>()))
                .Returns<RasDialContext>(c => {
                    Assert.That(c.OnStateChangedCallback, Is.Not.Null);
                    Assert.Throws<ArgumentNullException>(() => c.OnStateChangedCallback(null));

                    executed = true;
                    return Task.FromResult(result.Object);
                });

            var target = new RasDialer(api.Object) { EntryName = EntryName, PhoneBookPath = PhoneBookPath };

            await target.ConnectAsync();

            Assert.That(executed, Is.True);
        }

        [Test]
        public async Task RaisesTheEventFromTheOnStateChangedCallback() {
            var e = new StateChangedEventArgs(RasConnectionState.OpenPort);
            var result = new Mock<RasConnection>();

            var api = new Mock<IRasDial>();
            api.Setup(o => o.DialAsync(It.IsAny<RasDialContext>()))
                .Returns<RasDialContext>(c => {
                    Assert.That(c.OnStateChangedCallback, Is.Not.Null);
                    c.OnStateChangedCallback(e);

                    return Task.FromResult(result.Object);
                });

            var raised = false;

            var target = new RasDialer(api.Object) { EntryName = EntryName, PhoneBookPath = PhoneBookPath };

            target.StateChanged += (sender, args) => {
                Assert.That(args, Is.EqualTo(e));
                raised = true;
            };

            await target.ConnectAsync();

            Assert.That(raised, Is.True);
        }

        [Test]
        public void DisposesTheApiAsExpected() {
            var api = new Mock<IRasDial>();

            var target = new RasDialer(api.Object);
            target.Dispose();

            api.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public void DoesNotThrowsAnExceptionWhenThePhoneBookPathHasNotBeenSet() {
            var api = new Mock<IRasDial>();

            var target = new RasDialer(api.Object) { EntryName = EntryName, PhoneBookPath = null };

            Assert.DoesNotThrow(() => target.Connect());
        }

        [Test]
        public void IndicatesTheObjectIsBusyAsExpected() {
            var api = new Mock<IRasDial>();
            api.Setup(o => o.IsBusy).Returns(true);

            var target = new RasDialer(api.Object);
            Assert.That(target.IsBusy, Is.True);
        }

        [Test]
        public void IndicatesTheObjectIsNotBusyAsExpected() {
            var api = new Mock<IRasDial>();
            api.Setup(o => o.IsBusy).Returns(false);

            var target = new RasDialer(api.Object);
            Assert.That(target.IsBusy, Is.False);
        }
    }
}
