using System;
using Microsoft.Win32.SafeHandles;

namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface IRegisteredCallback : IDisposable
    {
        SafeWaitHandle Handle { get; }
    }
}