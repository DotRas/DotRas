using System;
using System.ComponentModel;
using System.Text;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services
{
    internal class RasGetErrorString : IRasGetErrorString
    {
        /// <summary>
        /// Defines the default buffer size as defined within the Microsoft documentation.
        /// </summary>
        private const int DefaultBufferSize = 1024;

        private readonly IRasApi32 api;

        public RasGetErrorString(IRasApi32 api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public string GetErrorString(int errorCode)
        {
            if (errorCode <= 0)
            {
                throw new ArgumentException("The error code must be a positive value.", nameof(errorCode));
            }

            var errorBuilder = new StringBuilder(DefaultBufferSize);

            var ret = api.RasGetErrorString(errorCode, errorBuilder, errorBuilder.Capacity);
            if (ret == ERROR_INVALID_PARAMETER)
            {
                return null;
            }

            if (ret != SUCCESS)
            {
                throw new Win32Exception(ret);
            }

            return errorBuilder.ToString();
        }
    }
}