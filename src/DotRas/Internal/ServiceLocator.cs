using System;
using DotRas.Internal.Infrastructure.IoC;

namespace DotRas.Internal
{
    internal static class ServiceLocator
    {
        private static readonly object SyncRoot = new object();
        private static Func<IServiceProvider> locator;

        private static readonly Container Container = ContainerBuilder.Build();

        static ServiceLocator()
        {
            Reset();
        }

        public static IServiceProvider Default => locator();

        public static void SetLocator(Func<IServiceProvider> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            lock (SyncRoot)
            {
                locator = func;
            }
        }

        public static void Reset()
        {
            SetLocator(() => Container);
        }
    }
}