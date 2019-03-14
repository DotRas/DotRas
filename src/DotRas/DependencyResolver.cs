using System;

namespace DotRas
{
    /// <summary>
    /// Provides an access-point to resolve external implementations of dependencies.
    /// </summary>
    public static class DependencyResolver
    {
        private static Func<IServiceProvider> current;
        
        /// <summary>
        /// Gets the current service provider.
        /// </summary>
        public static IServiceProvider Current => current?.Invoke();

        /// <summary>
        /// Sets the locator for a service provider.
        /// </summary>
        /// <param name="locator">The locator function to use when service location is required.</param>
        public static void SetLocator(Func<IServiceProvider> locator)
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