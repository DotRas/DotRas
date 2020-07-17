using System;

namespace DotRas.Diagnostics
{
    /// <summary>
    /// Provides a mechanism for locating the <see cref="ILogger"/> to use.
    /// </summary>
    public static class LoggerLocator
    {
        private static Func<ILogger> current;
        
        /// <summary>
        /// Gets the current logger.
        /// </summary>
        public static ILogger Current => current?.Invoke();

        /// <summary>
        /// Sets the locator.
        /// </summary>
        /// <param name="locator">The locator function to use when service location is required.</param>
        /// <exception cref="ArgumentNullException"><paramref name="locator"/>is a null reference.</exception>
        public static void SetLocator(Func<ILogger> locator)
        {
            current = locator ?? throw new ArgumentNullException(nameof(locator));
        }

        /// <summary>
        /// Clears the locator.
        /// </summary>
        public static void Clear()
        {
            current = null;
        }
    }
}