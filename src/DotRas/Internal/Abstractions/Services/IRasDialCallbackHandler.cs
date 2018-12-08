using System;
using System.Threading;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasDialCallbackHandler
    {
        void Initialize(ITaskCompletionSource<RasConnection> completionSource, Action<DialerStateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken);

        bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hrasconn, uint message, ConnectionState rascs, int dwError, int dwExtendedError);

        void SetHandle(RasHandle value);
    }
}