using System;

namespace DotRas.Internal.Infrastructure.IoC {
    internal static partial class ContainerExtensions {
        /// <summary>
        /// Registers a type as itself for implementation.
        /// </summary>
        /// <param name="container">The container instance.</param>
        /// <param name="type">The type to register.</param>
        /// <returns>The registered type.</returns>
        public static Container.IRegisteredType RegisterType(this Container container, Type type) => container.Register(type, type);
    }
}
