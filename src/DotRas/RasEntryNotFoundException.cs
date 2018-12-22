using System;
using System.Runtime.Serialization;

namespace DotRas
{
    /// <summary>
    /// Thrown when the entry does not exist.
    /// </summary>
    [Serializable]
    public class RasEntryNotFoundException : Exception
    {
        /// <summary>
        /// Gets the full path (including filename) of the phone book.
        /// </summary>
        public string PhoneBookPath { get; }

        /// <summary>
        /// Gets the name of the entry which caused the exception.
        /// </summary>
        public string EntryName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEntryNotFoundException"/> class.
        /// </summary>
        public RasEntryNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEntryNotFoundException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public RasEntryNotFoundException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHandleException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="entryName">The name of the entry that does not exist.</param>
        /// <param name="phoneBookPath">The full path (including filename) of the phone book.</param>
        public RasEntryNotFoundException(string message, string entryName, string phoneBookPath)
            : base(message)
        {
            EntryName = entryName;
            PhoneBookPath = phoneBookPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasEntryNotFoundException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which is the cause of this exception.</param>
        public RasEntryNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected RasEntryNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}