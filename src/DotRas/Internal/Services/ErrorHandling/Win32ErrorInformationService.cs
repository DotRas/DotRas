using System;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.RasError;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.ErrorHandling
{
    internal class Win32ErrorInformationService : IWin32ErrorInformation
    {
        private readonly IRasGetErrorString rasGetErrorString;
        private readonly IWin32FormatMessage win32FormatMessage;

        public Win32ErrorInformationService(IRasGetErrorString rasGetErrorString, IWin32FormatMessage win32FormatMessage)
        {
            this.rasGetErrorString = rasGetErrorString ?? throw new ArgumentNullException(nameof(rasGetErrorString));
            this.win32FormatMessage = win32FormatMessage ?? throw new ArgumentNullException(nameof(win32FormatMessage));
        }

        public Win32ErrorInformation CreateFromErrorCode(int errorCode)
        {
            if (errorCode == SUCCESS)
            {
                return null;
            }

            string errorMessage = null;
            if (ShouldGetErrorMessageFromRas(errorCode))
            {
                errorMessage = GetErrorMessageFromRas(errorCode);
            }
            
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = GetErrorMessageFromWin32(errorCode);
            }

            return new Win32ErrorInformation(
                errorCode, 
                errorMessage);
        }

        protected virtual bool ShouldGetErrorMessageFromRas(int errorCode)
        {
            return errorCode > RASBASE;
        }

        protected virtual string GetErrorMessageFromRas(int errorCode)
        {
            return rasGetErrorString.GetErrorString(errorCode);        
        }

        protected virtual string GetErrorMessageFromWin32(int errorCode)
        {
            return win32FormatMessage.FormatMessage(errorCode);
        }
    }
}