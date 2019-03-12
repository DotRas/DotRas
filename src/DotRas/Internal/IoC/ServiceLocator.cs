using System;
using System.Collections.Generic;
using DotRas.Internal.Abstractions.IoC;

namespace DotRas.Internal.IoC
{
    internal class ServiceLocator : IServiceProvider, ICompositionRegistry
    {
        private readonly IDictionary<Type, ICompositionFactory> factories = new Dictionary<Type, ICompositionFactory>();

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

        public void RegisterCallback<T>(Func<IServiceProvider, T> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            factories.Add(typeof(T), new CallbackFactoryAdapter<T>(callback));
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (!factories.TryGetValue(serviceType, out var factory))
            {
                throw new KeyNotFoundException($"The service type '{serviceType}' does not exist in the registry.");
            }

            return factory.CreateObject(this);
        }
    }
}