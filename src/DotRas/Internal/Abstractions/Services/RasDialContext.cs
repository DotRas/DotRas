using System;
using System.Threading;

namespace DotRas.Internal.Abstractions.Services
{
    internal struct RasDialContext
    {
        public string PhoneBookPath { get; set; }
        public string EntryName { get; set; }
        public RasDialerCredentials Credentials { get; set; }
        public Action<DialStateChangedEventArgs> OnStateChangedCallback { get; set; }
        public RasDialerOptions Options { get; set; }
        public CancellationToken CancellationToken { get; set; }        
    }
}