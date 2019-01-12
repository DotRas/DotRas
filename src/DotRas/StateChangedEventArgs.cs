using System;

namespace DotRas
{
    /// <summary>
    /// Provides event details for state changes while establishing a connection.
    /// </summary>
    public class StateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the state of the connection.
        /// </summary>
        public RasConnectionState State { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="state">The state of the connection.</param>
        public StateChangedEventArgs(RasConnectionState state)
        {
            State = state;
        }
    }
}