using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Dialing;
using DotRas.Tests.Stubs;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Tests.Internal.Services.Dialing
{
    [TestFixture]
    public class RasDialServiceTests
    {
        private delegate int RasDialCallback(
            ref RASDIALEXTENSIONS rasDialExtensions,
            string lpszPhoneBook,
            ref RASDIALPARAMS rasDialParams,
            Ras.NotifierType notifierType,
            RasDialFunc2 rasDialFunc,
            out IntPtr handle);

        private Mock<IRasApi32> api;
        private Mock<IRasHangUp> rasHangUp;
        private Mock<IRasDialExtensionsBuilder> extensionsBuilder;
        private Mock<IRasDialParamsBuilder> paramsBuilder;
        private Mock<IExceptionPolicy> exceptionPolicy;
        private Mock<IRasDialCallbackHandler> callbackHandler;

        [SetUp]
        public void Init()
        {
            api = new Mock<IRasApi32>();
            rasHangUp = new Mock<IRasHangUp>();
            extensionsBuilder = new Mock<IRasDialExtensionsBuilder>();
            paramsBuilder = new Mock<IRasDialParamsBuilder>();
            exceptionPolicy = new Mock<IExceptionPolicy>();
            callbackHandler = new Mock<IRasDialCallbackHandler>();
        }

        [Test]
        public void ThrowAnExceptionWhenTheApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new RasDialService(null, new Mock<IRasHangUp>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IRasDialCallbackHandler>().Object);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new RasDialService(new Mock<IRasApi32>().Object, new Mock<IRasHangUp>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, null, new Mock<IRasDialCallbackHandler>().Object);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheCallbackHandlerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new RasDialService(new Mock<IRasApi32>().Object, new Mock<IRasHangUp>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, new Mock<IExceptionPolicy>().Object, null);
            });
        }

        [Test]
        public void DisposesCorrectlyWhenNotInitialized()
        {
            var target = new RasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object);
            target.Dispose();

            callbackHandler.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public void HangsUpTheConnectionWhenCancelled()
        {
            var handle = new IntPtr(1);
            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, It.IsAny<string>(), ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny)).Returns(new RasDialCallback(    (ref RASDIALEXTENSIONS rasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS rasDialParams, Ras.NotifierType notifierType, RasDialFunc2 o5, out IntPtr o6) =>
                {
                    o6 = handle;
                    return SUCCESS;
                }));

            using var cts = new CancellationTokenSource();
            var target = new TestableRasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object);

            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                CancellationToken = cts.Token
            };

            target.DialAsync(context);

            cts.Cancel();

            rasHangUp.Verify(o => o.UnsafeHangUp(target.Handle, It.IsAny<bool>()));
        }

        [Test]
        public void DisposeMustDisposeTheCallbackHandler()
        {
            var target = new TestableRasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object);
            target.Dispose();

            callbackHandler.Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public async Task DialTheConnection()
        {
            var handle = new IntPtr(1);
            var phoneBookPath = @"C:\Test.pbk";
            var entryName = "Entry";
            var userName = "User";
            var password = "Password";
            var interfaceIndex = 1;

            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, phoneBookPath, ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny)).Returns(new RasDialCallback(
                (ref RASDIALEXTENSIONS rasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS rasDialParams, Ras.NotifierType notifierType, RasDialFunc2 o5, out IntPtr o6) =>
                {
                    Assert.AreEqual(phoneBookPath, lpszPhoneBook);
                    o6 = handle;

                    return SUCCESS;
                }));

            var connection = new Mock<RasConnection>();

            var completionSource = new TaskCompletionSource<RasConnection>();
            completionSource.SetResult(connection.Object);

            var context = new RasDialContext
            {
                PhoneBookPath = phoneBookPath,
                EntryName = entryName,
                Credentials = new NetworkCredential(userName, password),
                Options = new RasDialerOptions
                {
                    InterfaceIndex = interfaceIndex
                }
            };

            var target = new TestableRasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object)
            {
                CompletionSource = completionSource
            };

            var result = await target.DialAsync(context);

            Assert.AreSame(connection.Object, result);
            Assert.IsTrue(target.IsBusy);
            callbackHandler.Verify(o => o.Initialize(completionSource, It.IsAny<Action<StateChangedEventArgs>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
            callbackHandler.Verify(o => o.SetHandle(handle), Times.Once);
        }

        [Test]
        public void ThrowsAnExceptionWhenNonSuccessIsReturnedFromWin32()
        {
            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, @"C:\Test.pbk", ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny)).Returns(new RasDialCallback(
                (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, Ras.NotifierType o4, RasDialFunc2 o5, out IntPtr o6) =>
                {
                    o6 = IntPtr.Zero;

                    return ERROR_INVALID_PARAMETER;
                }));

            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

            var connection = new Mock<RasConnection>();
            var completionSource = new TaskCompletionSource<RasConnection>();
            completionSource.SetResult(connection.Object);

            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                Options = new RasDialerOptions
                {
                    InterfaceIndex = 0
                }
            };

            var target = new RasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object);
            Assert.ThrowsAsync<TestException>(() => target.DialAsync(context));
        }

        [Test]
        public void ThrowsAnExceptionWhenAttemptingToDialWhileAlreadyBusy()
        {
            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                Options = new RasDialerOptions
                {
                    InterfaceIndex = 0
                }
            };

            var target = new TestableRasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object);
            target.FlagAsBusy();

            Assert.IsTrue(target.IsBusy);
            Assert.ThrowsAsync<InvalidOperationException>(() => target.DialAsync(context));

            Assert.IsTrue(target.IsBusy);
        }

        [Test]
        public void CancelsTheDialConnectionAttemptIfDisposed()
        {
            var handle = new IntPtr(1);

            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, @"C:\Test.pbk", ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<IntPtr>.IsAny)).Returns(new RasDialCallback(
                (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, Ras.NotifierType o4, RasDialFunc2 o5, out IntPtr o6) =>
                {
                    o6 = handle;

                    return SUCCESS;
                }));

            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential("User", "Password"),
                Options = new RasDialerOptions
                {
                    InterfaceIndex = 0
                }
            };

            var target = new TestableRasDialService(api.Object, rasHangUp.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object);
            target.DialAsync(context);

            Assert.IsTrue(target.IsBusy);

            target.Dispose();
            Assert.IsTrue(target.CancelledAttempt);
        }
    }
}