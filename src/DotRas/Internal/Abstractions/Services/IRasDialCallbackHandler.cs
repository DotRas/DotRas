using System;
using System.Threading;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDialCallbackHandler : IDisposable
    {
        void Initialize(RasDialContext context, ITaskCompletionSource<RasConnection> completionSource, Action<StateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken);

        bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hrasconn, uint message, RasConnectionState rascs, int dwError, int dwExtendedError);

        void SetHandle(IntPtr handle);
    }
}