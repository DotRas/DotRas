using System;

namespace DotRas.Internal.Abstractions.Services
{
    internal class RasNotificationContext
    {
        public IRasConnection Connection { get; set; }

        public Action<RasConnectionEventArgs> OnConnectedCallback { get; set; }
        public Action<RasConnectionEventArgs> OnDisconnectedCallback { get; set; }
    }
}