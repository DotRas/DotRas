using System;
using System.Net;
using System.Threading;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Abstractions.Services
{
    internal class RasDialContext
    {
        public string PhoneBookPath { get; set; }
        public string EntryName { get; set; }
        public NetworkCredential Credentials { get; set; }
        public Action<StateChangedEventArgs> OnStateChangedCallback { get; set; }
        public RasDialerOptions Options { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IntPtr Handle { get; set; }
        public RASDIALPARAMS RasDialParams { get; set; }
        public RASDIALEXTENSIONS RasDialExtensions { get; set; }
    }
}