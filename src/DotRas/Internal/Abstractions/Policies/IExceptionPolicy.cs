using System;

namespace DotRas.Internal.Abstractions.Policies
{
    internal interface IExceptionPolicy
    {
        Exception Create(int error);
    }
}