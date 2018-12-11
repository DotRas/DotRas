using System;
using System.ComponentModel;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Policies
{
    internal class DefaultExceptionPolicy : IExceptionPolicy
    {
        private readonly IRasGetErrorString rasGetErrorString;

        public DefaultExceptionPolicy(IRasGetErrorString getErrorString)
        {
            this.rasGetErrorString = getErrorString ?? throw new ArgumentNullException(nameof(getErrorString));
        }

        public Exception Create(int error)
        {
            if (error == SUCCESS)
            {
                throw new ArgumentException("Not a valid error code.", nameof(error));
            }

            if (error == ERROR_INVALID_SIZE)
            {
                return new NotSupportedException("The operating system does not support the operation being requested. Please check the compatibility matrix for features supported with this operating system.");
            }

            var message = rasGetErrorString.GetErrorString(error);
            if (string.IsNullOrWhiteSpace(message))
            {
                return new Win32Exception(error);
            }

            return new RasException(error, message);
        }
    }
}