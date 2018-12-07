using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Internal.Services.Connections;
using DotRas.Tests.Internal.Stubs;
using DotRas.Tests.Stubs;
using DotRas.Win32;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.NativeMethods;
using static DotRas.Win32.WinError;

namespace DotRas.Tests.Internal.Services
{
    [TestFixture]
    public class RasDialTests
    {
        private delegate void RasDialCallback(
            ref RASDIALEXTENSIONS rasDialExtensions, 
            string phoneBook, 
            ref RASDIALPARAMS rasDialParams, 
            Ras.NotifierType notifierType, 
            RasDialFunc2 rasDialFunc, 
            out RasHandle handle);

        [Test]
        public void ThrowAnExceptionWhenTheApiIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasDial(null, new Mock<IStructFactory>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IRasDialCallbackHandler>().Object);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasDial(new Mock<IRasApi32>().Object, null, new Mock<IExceptionPolicy>().Object, new Mock<IRasDialCallbackHandler>().Object);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheExceptionPolicyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasDial(new Mock<IRasApi32>().Object, new Mock<IStructFactory>().Object, null, new Mock<IRasDialCallbackHandler>().Object);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheCallbackHandlerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new RasDial(new Mock<IRasApi32>().Object, new Mock<IStructFactory>().Object, new Mock<IExceptionPolicy>().Object, null);
            });
        }

        [Test]
        public void ThrowAnExceptionWhenTheContextIsNull()
        {
            var target = new RasDial(new Mock<IRasApi32>().Object, new Mock<IStructFactory>().Object, new Mock<IExceptionPolicy>().Object, new Mock<IRasDialCallbackHandler>().Object);
            Assert.ThrowsAsync<ArgumentNullException>(() => target.DialAsync(null));
        }

        [Test]
        public async Task DisposeWillDisposeTheCallbackHandler()
        {
            var completionSource = new Mock<ITaskCompletionSource<Connection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult((Connection)null));

            var api = new Mock<IRasApi32>();
            var structFactory = new Mock<IStructFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            callbackHandler.As<IDisposable>();

            using (var target = new StubRasDial(completionSource.Object, api.Object, structFactory.Object, exceptionPolicy.Object, callbackHandler.Object))
            {
                await target.DialAsync(new RasDialContext(@"C:\Test.pbk", "Entry", new NetworkCredential("User", "Password"), CancellationToken.None, null));
            }

            callbackHandler.As<IDisposable>().Verify(o => o.Dispose(), Times.Once);
        }

        [Test]
        public async Task DialTheConnection()
        {
            var handle = new RasHandle();

            var api = new Mock<IRasApi32>();
            api.Setup(o => o.RasDial(ref It.Ref<RASDIALEXTENSIONS>.IsAny, @"C:\Test.pbk", ref It.Ref<RASDIALPARAMS>.IsAny, Ras.NotifierType.RasDialFunc2, It.IsAny<RasDialFunc2>(), out It.Ref<RasHandle>.IsAny)).Callback(new RasDialCallback(
                (ref RASDIALEXTENSIONS o1, string o2, ref RASDIALPARAMS o3, Ras.NotifierType o4, RasDialFunc2 o5, out RasHandle o6) =>
                {
                    o6 = handle;
                }));

            var structFactory = new Mock<IStructFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            var connection = new Mock<Connection>();

            var completionSource = new Mock<ITaskCompletionSource<Connection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult(connection.Object));

            var target = new StubRasDial(completionSource.Object, api.Object, structFactory.Object, exceptionPolicy.Object, callbackHandler.Object);
            var result = await target.DialAsync(new RasDialContext(@"C:\Test.pbk", "Entry", new NetworkCredential("User", "Password"), CancellationToken.None, null));

            Assert.AreSame(connection.Object, result);
            Assert.IsTrue(target.IsBusy);
            callbackHandler.Verify(o => o.Initialize(completionSource.Object, It.IsAny<Action<DialerStateChangedEventArgs>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
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

            var structFactory = new Mock<IStructFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();
            exceptionPolicy.Setup(o => o.Create(ERROR_INVALID_PARAMETER)).Returns(new TestException());

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            var connection = new Mock<Connection>();

            var completionSource = new Mock<ITaskCompletionSource<Connection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult(connection.Object));

            var target = new StubRasDial(completionSource.Object, api.Object, structFactory.Object, exceptionPolicy.Object, callbackHandler.Object);
            Assert.ThrowsAsync<TestException>(() => target.DialAsync(new RasDialContext(@"C:\Test.pbk", "Entry", new NetworkCredential("User", "Password"), CancellationToken.None, null)));

            Assert.IsFalse(target.IsBusy);
            callbackHandler.Verify(o => o.Initialize(completionSource.Object, It.IsAny<Action<DialerStateChangedEventArgs>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
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

            var structFactory = new Mock<IStructFactory>();
            var exceptionPolicy = new Mock<IExceptionPolicy>();

            var callbackHandler = new Mock<IRasDialCallbackHandler>();
            var connection = new Mock<Connection>();

            var completionSource = new Mock<ITaskCompletionSource<Connection>>();
            completionSource.Setup(o => o.Task).Returns(Task.FromResult(connection.Object));

            var target = new StubRasDial(completionSource.Object, api.Object, structFactory.Object, exceptionPolicy.Object, callbackHandler.Object);
            await target.DialAsync(new RasDialContext(@"C:\Test.pbk", "Entry", new NetworkCredential("User", "Password"), CancellationToken.None, null));

            Assert.IsTrue(target.IsBusy);
            Assert.ThrowsAsync<InvalidOperationException>(() => target.DialAsync(new RasDialContext(@"C:\Test.pbk", "Entry", new NetworkCredential("User", "Password"), CancellationToken.None, null)));

            Assert.IsTrue(target.IsBusy);
        }
    }
}