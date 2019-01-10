using System;
using DotRas.Internal.Abstractions.Primitives;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Internal.Services.Connections
{
    internal class RasConnectionNotificationStateObject
    {
        public Action<RasConnectionEventArgs> Callback { get; set; }
        public IRegisteredCallback RegisteredCallback { get; set; }

        public RASCN NotificationType { get; set; }
    }
}