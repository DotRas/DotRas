namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasConnectionNotificationCallbackHandler
    {
        void Initialize();
        void OnCallback(object obj, bool timedOut);
    }
}