using System;

namespace DotRas
{
    /// <summary>
    /// Identifies a remote access service (RAS) connection.
    /// </summary>
    public interface IRasConnection
    {
        /// <summary>
        /// Gets the handle of the connection.
        /// </summary>
        IntPtr Handle { get; }
    }
}