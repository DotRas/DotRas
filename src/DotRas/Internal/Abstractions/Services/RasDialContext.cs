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
        public Action<DialStateChangedEventArgs> OnStateChangedCallback { get; }
        public int InterfaceIndex { get; }
        public CancellationToken CancellationToken { get; }

        public RasDialContext(string phoneBookPath, string entryName, NetworkCredential credentials, int interfaceIndex, Action<DialStateChangedEventArgs> onStateChangedCallback, CancellationToken cancellationToken)
        {
            PhoneBookPath = phoneBookPath;
            EntryName = entryName;
            Credentials = credentials;
            InterfaceIndex = interfaceIndex;
            OnStateChangedCallback = onStateChangedCallback;
            CancellationToken = cancellationToken;
        }
    }
}