namespace DotRas.Internal.Interop
{
    internal static class WinError
    {
        public const int SUCCESS = 0;

        public const int ERROR_INVALID_HANDLE = 6;

        public const int ERROR_INVALID_PARAMETER = 87;

        public const int ERROR_INSUFFICIENT_BUFFER = 122;

        public const int ERROR_ALREADY_EXISTS = 183;

        /// <summary>
        /// CUSTOM: Identifies the start range of the IPSec errors.
        /// </summary>
        public const int IPSECBASE = 13000;

        /// <summary>
        /// CUSTOM: Identifies the end range of the IPSec errors.
        /// </summary>
        public const int IPSECBASEEND = 13999;
    }
}