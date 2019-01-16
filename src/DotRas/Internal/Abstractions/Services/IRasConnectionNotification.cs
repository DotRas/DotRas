using System;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasConnectionNotification : IDisposable
    {
        void Subscribe(RasNotificationContext context);
        void Reset();
    }
}