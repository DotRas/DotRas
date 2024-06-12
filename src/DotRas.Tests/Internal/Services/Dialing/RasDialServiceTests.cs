using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Dialing;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Dialing {
    [TestFixture]
    public class RasDialServiceTests {
        private delegate int RasDialCallback(ref RASDIALEXTENSIONS rasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS rasDialParams, Ras.NotifierType notifierType, RasDialFunc2 rasDialFunc, out IntPtr handle);

        private TestableRasDialService target;

        private Mock<IRasApi32> api;
        private Mock<IRasHangUp> rasHangUp;
        private Mock<IRasDialExtensionsBuilder> extensionsBuilder;
        private Mock<IRasDialParamsBuilder> paramsBuilder;
        private Mock<IExceptionPolicy> exceptionPolicy;
        private Mock<IRasDialCallbackHandler> callbackHandler;
        private Mock<IMarshaller> marshaller;

        [SetUp]
        public void Init() {
            api = new Mock<IRasApi32>();
            rasHangUp = new Mock<IRasHangUp>();
            extensionsBuilder = new Mock<IRasDialExtensionsBuilder>();
            paramsBuilder = new Mock<IRasDialParamsBuilder>();
            exceptionPolicy = new Mock<IExceptionPolicy>();
            callbackHandler = new Mock<IRasDialCallbackHandler>();
            marshaller = new Mock<IMarshaller>();

            target = new TestableRasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object, marshaller.Object);
        }

        [TearDown]
        public void Finish() => target?.Dispose();

        [Test]
        public void ThrowAnExceptionWhenTheApiIsNull() =>
            Assert.Throws<ArgumentNullException>(() => {
                _ = new RasDialService(null, new Mock<IRasHangUp>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IRasDialCallbackHandler>().Object, marshaller.Object);
            });

        [Test]
        public void ThrowAnExceptionWhenTheExceptionPolicyIsNull() =>
            Assert.Throws<ArgumentNullException>(() => {
                _ = new RasDialService(new Mock<IRasApi32>().Object, new Mock<IRasHangUp>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, null, new Mock<IRasDialCallbackHandler>().Object, marshaller.Object);
            });

        [Test]
        public void ThrowAnExceptionWhenTheCallbackHandlerIsNull() =>
            Assert.Throws<ArgumentNullException>(() => {
                _ = new RasDialService(new Mock<IRasApi32>().Object, new Mock<IRasHangUp>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, new Mock<IExceptionPolicy>().Object, null, marshaller.Object);
            });

        [Test]
        public void DisposesCorrectlyWhenNotInitialized() => Assert.DoesNotThrow(() => target.Dispose());

        [Test]
        public void HangsUpTheConnectionWhenCancelled() {
            var handle = new IntPtr(1);
            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, It.IsAny<string>(), ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny))
                .Returns(
                    new RasDialCallback(
                        (ref RASDIALEXTENSIONS rasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS rasDialParams, Ras.NotifierType notifierType, RasDialFunc2 o5, out IntPtr o6) => {
                            o6 = handle;
                            return SUCCESS;
                        }
                    )
                );

            using var cts = new CancellationTokenSource();

            var context = new RasDialContext {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                CancellationToken = cts.Token
            };

            target.DialAsync(context);

            cts.Cancel();

            rasHangUp.Verify(o => o.UnsafeHangUp(context.Handle, It.IsAny<bool>(), CancellationToken.None));
        }

        [Test]
        public void DisposeMustDisposeTheCallbackHandler() {
            target.Dispose();

            callbackHandler.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public void ThrowsAnExceptionWhenContextIsNull() => Assert.Throws<ArgumentNullException>(() => target.DialAsync(null));

        [Test]
        public void ReleasesTheEapUserDataWhenCompleted() {
            var ptr = new IntPtr(1);
            var context = new RasDialContext {
                RasDialExtensions = new RASDIALEXTENSIONS {
                    RasEapInfo = new RASEAPINFO { pbEapInfo = ptr, dwSizeofEapInfo = 1 }
                }
            };

            target.SimulateDialCompleted(context);

            marshaller.Verify(o => o.FreeHGlobalIfNeeded(ptr));
        }

        [Test]
        public void FlagsTheServiceAsNoLongerBusyWhenCompleted() {
            var context = new RasDialContext();

            target.FlagAsBusy();

            Assert.That(target.IsBusy, Is.True);

            target.SimulateDialCompleted(context);

            Assert.That(target.IsBusy, Is.False);
        }

        [Test]
        public async Task DialTheConnection() {
            var handle = new IntPtr(1);
            var phoneBookPath = @"C:\Test.pbk";
            var entryName = "Entry";
            var userName = "User";
            var password = "Password";
            var interfaceIndex = 1;

            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, phoneBookPath, ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny))
                .Returns(
                    new RasDialCallback(
                        (ref RASDIALEXTENSIONS rasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS rasDialParams, Ras.NotifierType notifierType, RasDialFunc2 o5, out IntPtr o6) => {
                            Assert.That(lpszPhoneBook, Is.EqualTo(phoneBookPath));
                            o6 = handle;

                            return SUCCESS;
                        }
                    )
                );

            var connection = new Mock<RasConnection>();

            var completionSource = new TaskCompletionSource<RasConnection>();
            completionSource.SetResult(connection.Object);

            var context = new RasDialContext {
                PhoneBookPath = phoneBookPath,
                EntryName = entryName,
                Credentials = new NetworkCredential(userName, password),
                Options = new RasDialerOptions { InterfaceIndex = interfaceIndex }
            };

            target.SetCompletionSource(completionSource);

            var result = await target.DialAsync(context);

            Assert.Multiple(() => {
                Assert.That(result, Is.SameAs(connection.Object));
                Assert.That(target.IsBusy, Is.True);
            });
            callbackHandler.Verify(o => o.Initialize(completionSource, It.IsAny<Action<StateChangedEventArgs>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
            callbackHandler.Verify(o => o.SetHandle(handle), Times.Once);
        }

        [Test]
        public void ThrowsAnExceptionWhenNonSuccessIsReturnedFromWin32() {
            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, @"C:\Test.pbk", ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny))
                .Returns(
                    new RasDialCallback(
                        (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, Ras.NotifierType o4, RasDialFunc2 o5, out IntPtr o6) => {
                            o6 = IntPtr.Zero;

                            return ERROR_INVALID_PARAMETER;
                        }
                    )
                );

            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

            var connection = new Mock<RasConnection>();
            var completionSource = new TaskCompletionSource<RasConnection>();
            completionSource.SetResult(connection.Object);

            var context = new RasDialContext {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                Options = new RasDialerOptions { InterfaceIndex = 0 }
            };

            Assert.ThrowsAsync<TestException>(() => target.DialAsync(context));
        }

        [Test]
        public void ThrowsAnExceptionWhenAttemptingToDialWhileAlreadyBusy() {
            var context = new RasDialContext {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                Options = new RasDialerOptions { InterfaceIndex = 0 }
            };

            target.FlagAsBusy();

            Assert.That(target.IsBusy, Is.True);
            Assert.ThrowsAsync<InvalidOperationException>(() => target.DialAsync(context));

            Assert.That(target.IsBusy, Is.True);
        }

        [Test]
        public void CancelsTheDialConnectionAttemptIfDisposed() {
            var handle = new IntPtr(1);

            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, @"C:\Test.pbk", ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny))
                .Returns(
                    new RasDialCallback(
                        (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, Ras.NotifierType o4, RasDialFunc2 o5, out IntPtr o6) => {
                            o6 = handle;

                            return SUCCESS;
                        }
                    )
                );

            using var cts = new CancellationTokenSource();

            var context = new RasDialContext {
                CancellationToken = cts.Token,
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                Options = new RasDialerOptions { InterfaceIndex = 0 }
            };

            target.DialAsync(context);

            Assert.That(target.IsBusy, Is.True);

            target.Dispose();
            Assert.That(target.CancelledAttempt, Is.True);
        }

        [Test]
        public void DoesNotErrorIfCancelledBeforeDialAttemptCanStart() {
            target.FlagAsBusy();

            var context = new RasDialContext {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                Options = new RasDialerOptions { InterfaceIndex = 0 }
            };

            Assert.DoesNotThrow(() => target.SimulateCancellationRequested(context));
            Assert.That(target.IsBusy, Is.False);
        }
    }
}
