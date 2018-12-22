using System;

namespace DotRas
{
    /// <summary>
    /// Contains extension methods for the <see cref="RasConnection"/> class.
    /// </summary>
    public static class RasConnectionExtensions
    {
        /// <summary>
        /// Determines whether the current user is the owner of the connection.
        /// </summary>
        /// <param name="connection">The connection to check.</param>
        /// <returns>true if the current user owns the connection, otherwise false.</returns>
        public static bool IsOwner(this RasConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            return connection.Options.IsOwnerCurrentUser && connection.Options.IsOwnerKnown;
        }

        /// <summary>
        /// Determines whether the current user is not the owner of the connection.
        /// </summary>
        /// <param name="connection">The connection to check.</param>
        /// <returns>true if the current user does not own the connection, otherwise false.</returns>
        public static bool IsNotOwner(this RasConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            return !connection.Options.IsOwnerCurrentUser && connection.Options.IsOwnerKnown;
        }
    }
}