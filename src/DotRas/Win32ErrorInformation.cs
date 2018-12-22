namespace DotRas
{
    /// <summary>
    /// Provides details of a Win32 error which occurred.
    /// </summary>
    public class Win32ErrorInformation
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        public virtual int ErrorCode { get; }

        /// <summary>
        /// Gets the message describing the error.
        /// </summary>
        public virtual string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32ErrorInformation"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message describing the error.</param>
        public Win32ErrorInformation(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Win32ErrorInformation"/> class.
        /// </summary>
        protected Win32ErrorInformation()
        {
        }
    }
}