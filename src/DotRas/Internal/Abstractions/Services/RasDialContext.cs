using System;
using System.Net;
using System.Threading;

namespace DotRas.Internal.Abstractions.Services
{
    internal struct RasDialContext
    {
        public string PhoneBookPath { get; }
        public string EntryName { get; }
        public NetworkCredential Credentials { get; }
        public Action<StateChangedEventArgs> OnStateChangedCallback { get; }
        public CancellationToken CancellationToken { get; }

        public RasDialContext(string phoneBookPath, string entryName, NetworkCredential credentials, Action<StateChangedEventArgs> onStateChangedCallback, CancellationToken cancellationToken)
        {
            PhoneBookPath = phoneBookPath;
            EntryName = entryName;
            Credentials = credentials;
            OnStateChangedCallback = onStateChangedCallback;
            CancellationToken = cancellationToken;
        }
    }
}