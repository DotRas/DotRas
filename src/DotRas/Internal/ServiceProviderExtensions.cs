using System;

namespace DotRas.Internal {
    internal static class ServiceProviderExtensions {
        public static T GetService<T>(this IServiceProvider serviceProvider) => (T)serviceProvider.GetService(typeof(T));

        public static object GetRequiredService(this IServiceProvider serviceProvider, Type serviceType) {
            if (serviceType == null) {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var result = serviceProvider.GetService(serviceType);
            return result == null ? throw new InvalidOperationException($"The required service '{serviceType}' could not be found.") : result;
        }

        public static T GetRequiredService<T>(this IServiceProvider serviceProvider) => (T)serviceProvider.GetRequiredService(typeof(T));
    }
}
