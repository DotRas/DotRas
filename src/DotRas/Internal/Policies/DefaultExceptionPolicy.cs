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

        public DefaultExceptionPolicy(IRasGetErrorString rasGetErrorString)
        {
            this.rasGetErrorString = rasGetErrorString ?? throw new ArgumentNullException(nameof(rasGetErrorString));
        }

        public virtual Exception Create(int error)
        {
            if (error == SUCCESS)
            {
                throw new ArgumentException("Not a valid error code.", nameof(error));
            }

            if (error == ERROR_INVALID_SIZE)
            {
                return new NotSupportedException("The operating system does not support the operation being requested. Please check the compatibility matrix for features supported with this operating system.");
            }

            if (error == ERROR_CANCELLED)
            {
                return new OperationCanceledException();
            }

            if (ShouldGetMessageFromRas(error))
            {
                return CreateExceptionFromRas(error);
            }

            return new Win32Exception(error);
        }

        private bool ShouldGetMessageFromRas(int error)
        {
            return error >= RASBASE && error <= ERROR_DEVICE_COMPLIANCE;
        }

        private Exception CreateExceptionFromRas(int error)
        {
            var message = rasGetErrorString.GetErrorString(error);
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Unknown error.";
            }

            return new RasException(error, message);
        }
    }
}