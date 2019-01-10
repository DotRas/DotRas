using System;

namespace DotRas
{
    /// <summary>
    /// Contains event data for connection related events.
    /// </summary>
    public class RasConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the connection associated with the event.
        /// </summary>
        public RasConnection Connection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionEventArgs"/> class.
        /// </summary>
        /// <param name="connection">The connection associated with the event.</param>
        public RasConnectionEventArgs(RasConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
    }
}
