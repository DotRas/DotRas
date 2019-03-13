using System;

namespace DotRas.Internal.IoC
{
    internal static class ServiceLocator
    {
        private static readonly object SyncRoot = new object();
        private static IServiceProvider @default;

        public static IServiceProvider Default
        {
            get
            {
                if (@default == null)
                {
                    lock (SyncRoot)
                    {
                        if (@default == null)
                        {
                            @default = ContainerBuilder.Build();
                        }
                    }
                }

                return @default;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                lock (SyncRoot)
                {
                    @default = value;
                }
            }
        }

        public static void Clear()
        {
            lock (@default)
            {
                @default = null;
            }
        }       
    }
}