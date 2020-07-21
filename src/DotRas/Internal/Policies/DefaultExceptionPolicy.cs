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
                return new OperatingSystemNotSupportedException();
            }

            if (IsRasErrorCode(error))
            {
                return CreateRasException(error);
            }
            else if (IsIPSecErrorCode(error))
            {
                return CreateIPSecException(error);
            }

            return new Win32Exception(error);
        }

        private bool IsRasErrorCode(int error)
        {
            return error >= RASBASE && error <= RASBASEEND;
        }

        private bool IsIPSecErrorCode(int error)
        {
            return error >= IPSECBASE && error <= IPSECBASEEND;
        }

        private Exception CreateRasException(int error)
        {
            var message = rasGetErrorString.GetErrorString(error);
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Unknown error.";
            }

            return new RasException(error, message);
        }

        private Exception CreateIPSecException(int error)
        {
            return new IPSecException(error);
        }
    }
}