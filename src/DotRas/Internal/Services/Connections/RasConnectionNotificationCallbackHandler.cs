using System;
using System.Collections.Generic;
using System.Linq;
using DotRas.Internal.Abstractions.Services;

namespace DotRas.Internal.Services.Connections
{
    internal class RasConnectionNotificationCallbackHandler : IRasConnectionNotificationCallbackHandler
    {
        private readonly IRasEnumConnections enumConnectionsService;
        private readonly object syncRoot = new object();

        private RasConnection[] previous;

        public RasConnectionNotificationCallbackHandler(IRasEnumConnections enumConnectionService)
        {
            this.enumConnectionsService = enumConnectionService ?? throw new ArgumentNullException(nameof(enumConnectionService));
        }

        public void Initialize()
        {
            lock (syncRoot)
            {
                previous = enumConnectionsService.EnumerateConnections().ToArray();
            }
        }

        public void OnCallback(object obj, bool timedOut)
        {
            if (!(obj is RasConnectionNotificationStateObject registration))
            {
                return;
            }

            lock (syncRoot)
            {
                var current = enumConnectionsService.EnumerateConnections().ToArray();
                var changes = FindChanges(current);

                if (changes.Any())
                {
                    ExecuteCallbackForChanges(registration.Callback, changes);
                    previous = current;
                }
            }
        }

        private IEnumerable<RasConnection> FindChanges(RasConnection[] current)
        {
            if (current.Length > previous.Length)
            {
                return FindChanges(current, previous);
            }

            return FindChanges(previous, current);
        }

        private static IEnumerable<RasConnection> FindChanges(IEnumerable<RasConnection> collectionA, IEnumerable<RasConnection> collectionB)
        {
            return collectionA.Where(o => !collectionB.Contains(o)).ToArray();
        }

        private void ExecuteCallbackForChanges(Action<RasConnectionEventArgs> callback, IEnumerable<RasConnection> changes)
        {
            if (changes == null || !changes.Any())
            {
                return;
            }

            foreach (var connection in changes)
            {
                callback.Invoke(new RasConnectionEventArgs(connection));
            }
        }
    }
}