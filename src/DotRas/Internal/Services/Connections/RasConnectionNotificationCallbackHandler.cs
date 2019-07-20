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

        private bool initialized;
        private RasConnection[] previousState;

        public RasConnectionNotificationCallbackHandler(IRasEnumConnections enumConnectionsService)
        {
            this.enumConnectionsService = enumConnectionsService ?? throw new ArgumentNullException(nameof(enumConnectionsService));
        }

        public void Initialize()
        {
            if (initialized)
            {
                return;
            }

            lock (syncRoot)
            {
                if (initialized)
                {
                    return;
                }

                previousState = enumConnectionsService.EnumerateConnections().ToArray();
                initialized = true;
            }
        }        

        public void OnCallback(object obj, bool timedOut)
        {
            if (obj is RasConnectionNotificationStateObject state)
            {
                OnCallback(state);
            }
        }

        private void OnCallback(RasConnectionNotificationStateObject state)
        {
            lock (syncRoot)
            {
                var current = enumConnectionsService.EnumerateConnections().ToArray();

                var changes = FindConnectionChanges(current);
                if (changes.Any())
                {
                    ExecuteCallbackForChanges(state.Callback, changes);
                }

                previousState = current;
            }
        }

        private IList<RasConnection> FindConnectionChanges(RasConnection[] current)
        {
            if (current.Length > previousState.Length)
            {
                return FindChanges(current, previousState);
            }

            return FindChanges(previousState, current);
        }

        private static IList<RasConnection> FindChanges(IEnumerable<RasConnection> collectionA, IEnumerable<RasConnection> collectionB)
        {
            return collectionA.Where(o => !collectionB.Contains(o)).ToArray();
        }

        private void ExecuteCallbackForChanges(Action<RasConnectionEventArgs> callback, IEnumerable<RasConnection> changes)
        {
            foreach (var connection in changes)
            {
                callback.Invoke(new RasConnectionEventArgs(
                    new RasConnectionInformation(
                        connection.Handle,
                        connection.EntryName,
                        connection.PhoneBookPath,
                        connection.EntryId,
                        connection.CorrelationId)));
            }
        }
    }
}