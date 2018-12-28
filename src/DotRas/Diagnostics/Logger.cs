using System;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Provides a means to inject a custom logging implementation into the framework.
    /// </summary>
    public static class Logger
    {
        private static Func<ILog> current;
        
        /// <summary>
        /// Gets the current logger.
        /// </summary>
        public static ILog Current => current?.Invoke();

        /// <summary>
        /// Sets the locator for a logger.
        /// </summary>
        /// <param name="locator">The locator function to use when a logger is required.</param>
        public static void SetLocator(Func<ILog> locator)
        {
            current = locator ?? throw new ArgumentNullException(nameof(locator));
        }

        /// <summary>
        /// Clears the locator for a logger.
        /// </summary>
        public static void Clear()
        {
            current = null;
        }
    }
}