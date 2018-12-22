using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
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
        private delegate void RasDialCallback(
            ref RASDIALEXTENSIONS rasDialExtensions, 
            string lpszPhoneBook, 
            ref RASDIALPARAMS rasDialParams, 
            Ras.NotifierType notifierType, 
            RasDialFunc2 rasDialFunc, 
            out RasHandle handle);

        [Test]
        public void ThrowAnExceptionWhenTheApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasDialService(null, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IRasDialCallbackHandler>().Object, new Mock<ITaskCompletionSourceFactory>().Object);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasDialService(new Mock<IRasApi32>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, null, new Mock<IRasDialCallbackHandler>().Object, new Mock<ITaskCompletionSourceFactory>().Object);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheCallbackHandlerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasDialService(new Mock<IRasApi32>().Object, new Mock<IRasDialExtensionsBuilder>().Object, new Mock<IRasDialParamsBuilder>().Object, new Mock<IExceptionPolicy>().Object, null, new Mock<ITaskCompletionSourceFactory>().Object);
            });
        }

        [Test]
        public void ThrowsAnExceptionWhenTheCompletionSourceIsNotCreated()
        {
            var api = new Mock<IRasApi32>();
            var extensionsBuilder = new Mock<IRasDialExtensionsBuilder>();
            var paramsBuilder = new Mock<IRasDialParamsBuilder>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var completionSourceFactory = new Mock<ITaskCompletionSourceFactory>();
            completionSourceFactory.Setup(o => o.Create<RasConnection>()).Returns((ITaskCompletionSource<RasConnection>)null);

            var callbackHandler = new Mock<IRasDialCallbackHandler>();

            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential(
                    "User",
                    "Password"),
                InterfaceIndex = 0
            };

            var target = new RasDialService(api.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object, completionSourceFactory.Object);
            Assert.ThrowsAsync<InvalidOperationException>(() => target.DialAsync(context));
        }

        [Test]
        public async Task DisposeWillDisposeTheCallbackHandler()
        {
            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult((RasConnection)null));

            var api = new Mock<IRasApi32>();
            var extensionsBuilder = new Mock<IRasDialExtensionsBuilder>();
            var paramsBuilder = new Mock<IRasDialParamsBuilder>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var completionSourceFactory = new Mock<ITaskCompletionSourceFactory>();
            completionSourceFactory.Setup(o => o.Create<RasConnection>()).Returns(completionSource.Object);

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            callbackHandler.As<IDisposable>();

            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential(
                    "User",
                    "Password"),
                InterfaceIndex = 0
            };

            using (var target = new RasDialService(api.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object, completionSourceFactory.Object))
            {
                await target.DialAsync(context);
            }

            callbackHandler.As<IDisposable>().Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public async Task DialTheConnection()
        {
            var handle = new RasHandle();
            var phoneBookPath = @"C:\Test.pbk";
            var entryName = "Entry";
            var userName = "User";
            var password = "Password";
            var interfaceIndex = 1;

            var api = new Mock<IRasApi32>();

            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, phoneBookPath, ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<RasHandle>.IsAny)).Callback(new RasDialCallback(
                (ref RASDIALEXTENSIONS rasDialExtensions, string lpszPhoneBook, ref RASDIALPARAMS rasDialParams, Ras.NotifierType notifierType, RasDialFunc2 o5, out RasHandle o6) =>
                {
                    Assert.AreEqual(phoneBookPath, lpszPhoneBook);
                    o6 = handle;
                }));

            var extensionsBuilder = new Mock<IRasDialExtensionsBuilder>();
            var paramsBuilder = new Mock<IRasDialParamsBuilder>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            var connection = new Mock<RasConnection>();

            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult(connection.Object));

            var completionSourceFactory = new Mock<ITaskCompletionSourceFactory>();
            completionSourceFactory.Setup(o => o.Create<RasConnection>()).Returns(completionSource.Object);

            var context = new RasDialContext
            {
                PhoneBookPath = phoneBookPath,
                EntryName = entryName,
                Credentials = new NetworkCredential(
                    userName,
                    password),
                InterfaceIndex = interfaceIndex
            };

            var target = new RasDialService(api.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object, completionSourceFactory.Object);
            var result = await target.DialAsync(context);

            Assert.AreSame(connection.Object, result);
            Assert.IsTrue(target.IsBusy);
            callbackHandler.Verify(o => o.Initialize(completionSource.Object, It.IsAny<Action<DialStateChangedEventArgs>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
            callbackHandler.Verify(o => o.SetHandle(handle), Times.Once);
        }

        [Test]
        public void ThrowsAnExceptionWhenNonSuccessIsReturnedFromWin32()
        {
            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, @"C:\Test.pbk", ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<RasHandle>.IsAny)).Callback(new RasDialCallback(
                (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, Ras.NotifierType o4, RasDialFunc2 o5, out RasHandle o6) =>
                {
                    o6 = null;
                })).Returns(ERROR_INVALID_PARAMETER);

            var extensionsBuilder = new Mock<IRasDialExtensionsBuilder>();
            var paramsBuilder = new Mock<IRasDialParamsBuilder>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            var connection = new Mock<RasConnection>();

            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult(connection.Object));

            var completionSourceFactory = new Mock<ITaskCompletionSourceFactory>();
            completionSourceFactory.Setup(o => o.Create<RasConnection>()).Returns(completionSource.Object);

            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential(
                    "User",
                    "Password"),
                InterfaceIndex = 0
            };

            var target = new RasDialService(api.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object, completionSourceFactory.Object);
            Assert.ThrowsAsync<TestException>(() => target.DialAsync(context));

            Assert.IsFalse(target.IsBusy);
            callbackHandler.Verify(o => o.Initialize(completionSource.Object, It.IsAny<Action<DialStateChangedEventArgs>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
            callbackHandler.Verify(o => o.SetHandle(It.IsAny<RasHandle>()), Times.Never);
        }

        [Test]
        public async Task ThrowsAnExceptionWhenAttemptingToDialWhileAlreadyBusy()
        {
            var handle = new RasHandle();

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, @"C:\Test.pbk", ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<RasHandle>.IsAny)).Callback(new RasDialCallback(
                (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, Ras.NotifierType o4, RasDialFunc2 o5, out RasHandle o6) =>
                {
                    o6 = handle;
                }));

            var extensionsBuilder = new Mock<IRasDialExtensionsBuilder>();
            var paramsBuilder = new Mock<IRasDialParamsBuilder>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            var connection = new Mock<RasConnection>();

            var completionSource = new Mock<ITaskCompletionSource<RasConnection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult(connection.Object));

            var completionSourceFactory = new Mock<ITaskCompletionSourceFactory>();
            completionSourceFactory.Setup(o => o.Create<RasConnection>()).Returns(completionSource.Object);

            var context = new RasDialContext
            {
                PhoneBookPath = @"C:\Test.pbk",
                EntryName = "Entry",
                Credentials = new NetworkCredential(
                    "User",
                    "Password"),
                InterfaceIndex = 0
            };

            var target = new RasDialService(api.Object, extensionsBuilder.Object, paramsBuilder.Object, exceptionPolicy.Object, callbackHandler.Object, completionSourceFactory.Object);
            await target.DialAsync(context);

            Assert.IsTrue(target.IsBusy);
            Assert.ThrowsAsync<InvalidOperationException>(() => target.DialAsync(context));

            Assert.IsTrue(target.IsBusy);
        }
    }
}