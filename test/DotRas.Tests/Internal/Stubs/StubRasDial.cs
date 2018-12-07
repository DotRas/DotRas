using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Internal.Services.Connections;
using DotRas.Win32;

namespace DotRas.Tests.Internal.Stubs
{
    internal class StubRasDial : RasDial
    {
        private readonly ITaskCompletionSource<Connection> completionSource;

        public StubRasDial(ITaskCompletionSource<Connection> completionSource, IRasApi32 api, IStructFactory structFactory, IExceptionPolicy exceptionPolicy, IRasDialCallbackHandler callbackHandler) :
            base(api, structFactory, exceptionPolicy, callbackHandler)
        {
            this.completionSource = completionSource ?? throw new ArgumentNullException(nameof(completionSource));
        }

        protected override ITaskCompletionSource<Connection> CreateCompletionSource()
        {
            return completionSource;
        }
    }
}