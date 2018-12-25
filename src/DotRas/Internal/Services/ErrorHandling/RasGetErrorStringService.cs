using System;
using System.ComponentModel;
using System.Text;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.ErrorHandling
{
    internal class RasGetErrorStringService : IRasGetErrorString
    {
        /// <summary>
        /// Defines the default buffer size as defined within the Microsoft documentation.
        /// </summary>
        private const int DefaultBufferSize = 1024;

        private readonly IRasApi32 api;

        public RasGetErrorStringService(IRasApi32 api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public string GetErrorString(int errorCode)
        {
            var errorBuilder = new StringBuilder(DefaultBufferSize);

            var ret = api.RasGetErrorString(errorCode, errorBuilder, errorBuilder.Capacity);
            if (ret == ERROR_INSUFFICIENT_BUFFER)
            {
                throw new Win32Exception(ret);
            }
            else if (ret != SUCCESS)
            {
                return null;
            }

            return errorBuilder.ToString();
        }
    }
}