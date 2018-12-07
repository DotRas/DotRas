using System;

namespace DotRas
{
    internal static class ObjectExtensions
    {
        public static void DisposeIfNecessary(this object value)
        {
            if (value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}