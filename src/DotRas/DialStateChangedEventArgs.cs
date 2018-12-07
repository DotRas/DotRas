using System;
using DotRas.Win32.SafeHandles;

namespace DotRas
{
    /// <summary>
    /// Provides event details for state changes while dialing a connection.
    /// </summary>
    public class DialerStateChangedEventArgs : EventArgs
    {
        public ConnectionState State { get; }

        public DialerStateChangedEventArgs(ConnectionState state)
        {
            State = state;
        }
    }
}