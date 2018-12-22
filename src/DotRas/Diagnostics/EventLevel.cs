namespace DotRas.Diagnostics
{
    /// <summary>
    /// Defines the levels of events.
    /// </summary>
    public enum EventLevel
    {
        /// <summary>
        /// A critical event has occurred.
        /// </summary>
        Critical,

        /// <summary>
        /// An error has occurred.
        /// </summary>
        Error,

        /// <summary>
        /// A warning has been identified.
        /// </summary>
        Warning,

        /// <summary>
        /// An informational message has occurred.
        /// </summary>
        Information,

        /// <summary>
        /// A diagnostic message has occurred.
        /// </summary>
        Verbose
    }
}