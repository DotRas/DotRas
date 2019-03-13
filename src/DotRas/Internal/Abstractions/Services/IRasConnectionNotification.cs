using System;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasConnectionNotification : IDisposable
    {
        bool IsActive { get; }

        void Subscribe(RasNotificationContext context);
        void Reset();
    }
}