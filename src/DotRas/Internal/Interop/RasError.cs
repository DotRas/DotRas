namespace DotRas.Internal.Interop
{
    internal static class RasError
    {
        public const int RASBASE = 600;

        public const int ERROR_BUFFER_TOO_SMALL = RASBASE + 3;

        public const int ERROR_BUFFER_INVALID = RASBASE + 10;

        public const int ERROR_INVALID_SIZE = RASBASE + 32;

        public const int ERROR_NO_CONNECTION = RASBASE + 68;
    }
}