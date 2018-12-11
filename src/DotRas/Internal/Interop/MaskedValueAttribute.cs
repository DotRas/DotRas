using System;

namespace DotRas.Internal.Interop
{
    /// <summary>
    /// Identifies a field which should be masked during logging operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class MaskedValueAttribute : Attribute
    {
    }
}