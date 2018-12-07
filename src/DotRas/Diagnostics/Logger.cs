using System;

namespace DotRas.Diagnostics
{
    public static class Logger
    {
        private static Func<ILog> current;

        public static ILog Current
        {
            get { return current?.Invoke(); }
        }

        public static void SetLocator(Func<ILog> locator)
        {
            current = locator ?? throw new ArgumentNullException(nameof(locator));
        }
    }
}