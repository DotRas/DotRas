using System;
using System.Net;
using System.Threading;

namespace DotRas.Internal.Abstractions.Services
{
    internal struct RasDialContext
    {
        public string PhoneBookPath { get; set; }
        public string EntryName { get; set; }
        public NetworkCredential Credentials { get; set; }
        public Action<StateChangedEventArgs> OnStateChangedCallback { get; set; }
        public RasDialerOptions Options { get; set; }
        public CancellationToken CancellationToken { get; set; }        
    }
}