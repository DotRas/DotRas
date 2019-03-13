using System;

namespace DotRas
{
    /// <summary>
    /// Contains event data for connection change events.
    /// </summary>
    public class RasConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the connection information associated with the event.
        /// </summary>
        public RasConnectionInformation ConnectionInformation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionEventArgs"/> class.
        /// </summary>
        /// <param name="connectionInformation">The connection information associated with the event.</param>
        public RasConnectionEventArgs(RasConnectionInformation connectionInformation)
        {
            ConnectionInformation = connectionInformation ?? throw new ArgumentNullException(nameof(connectionInformation));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionEventArgs"/> class.
        /// </summary>
        protected RasConnectionEventArgs()
        {
        }
    }
}