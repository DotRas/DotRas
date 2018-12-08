using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Internal.Services.Connections;
using DotRas.Win32.SafeHandles;

namespace DotRas.Tests.Internal.Stubs
{
    internal class StubDefaultRasDialCallbackHandler : DefaultRasDialCallbackHandler
    {
        private readonly Func<RasHandle, RasConnection> connectionFactory;

        public StubDefaultRasDialCallbackHandler(Func<RasHandle, RasConnection> connectionFactory, IRasHangUp rasHangUp, IRasEnumConnections rasEnumConnections, IExceptionPolicy exceptionPolicy, IValueWaiter<RasHandle> handle, ITaskCancellationSourceFactory cancellationSourceFactory)
            : base(rasHangUp, rasEnumConnections, exceptionPolicy, handle, cancellationSourceFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        protected override RasConnection CreateConnection(RasHandle handle)
        {
            return connectionFactory(handle);
        }
    }
}