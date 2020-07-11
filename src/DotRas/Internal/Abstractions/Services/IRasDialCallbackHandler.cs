using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDialCallbackHandler : IDisposable
    {
        void Initialize(TaskCompletionSource<RasConnection> completionSource, Action<StateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken);

        bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hrasconn, uint message, RasConnectionState rascs, int dwError, int dwExtendedError);

        void SetHandle(IntPtr handle);
    }
}