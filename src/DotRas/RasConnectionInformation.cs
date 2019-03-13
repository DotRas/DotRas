using System;

namespace DotRas
{
    /// <summary>
    /// Provides information for a remote access service connection.
    /// </summary>
    public class RasConnectionInformation
    {
        #region Fields and Properties

        /// <summary>
        /// Gets the handle of the connection.
        /// </summary>
        public virtual IntPtr Handle { get; }

        /// <summary>
        /// Gets the name of the phone book entry used to establish the remote access connection.
        /// </summary>
        public virtual string EntryName { get; }

        /// <summary>
        /// Gets the full path (including filename) to the phone book containing the entry for this connection.
        /// </summary>
        public virtual string PhoneBookPath { get; }

        /// <summary>
        /// Gets the <see cref="Guid"/> that represents the phone book entry.
        /// </summary>
        public virtual Guid EntryId { get; }

        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public virtual Guid CorrelationId { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RasConnectionInformation"/> class.
        /// </summary>
        /// <param name="handle">The handle of the connection.</param>
        /// <param name="entryName">The phone book entry used to establish the remote access connection.</param>
        /// <param name="phoneBookPath">The full path (including filename) to the phone book containing the entry for this connection.</param>
        /// <param name="entryId">The <see cref="Guid"/> that represents the phone book entry.</param>
        /// <param name="correlationId">The correlation id.</param>
        public RasConnectionInformation(IntPtr handle, string entryName, string phoneBookPath, Guid entryId, Guid correlationId)
        {
            Handle = handle;
            EntryName = entryName;
            PhoneBookPath = phoneBookPath;
            EntryId = entryId;
            CorrelationId = correlationId;
        }
    }
}