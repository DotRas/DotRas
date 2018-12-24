using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.Dialing;

namespace DotRas.Tests.Internal.Stubs
{
    internal class StubDefaultRasDialCallbackHandler : DefaultRasDialCallbackHandler
    {
        private readonly Func<IntPtr, RasConnection> connectionFactory;

        public StubDefaultRasDialCallbackHandler(Func<IntPtr, RasConnection> connectionFactory, IRasHangUp rasHangUp, IRasEnumConnections rasEnumConnections, IExceptionPolicy exceptionPolicy, IValueWaiter<IntPtr> handle, ITaskCancellationSourceFactory cancellationSourceFactory)
            : base(rasHangUp, rasEnumConnections, exceptionPolicy, handle, cancellationSourceFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        protected override RasConnection CreateConnection(IntPtr handle)
        {
            return connectionFactory(handle);
        }
    }
}