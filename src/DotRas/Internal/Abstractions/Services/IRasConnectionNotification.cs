using System;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasConnectionNotification : IDisposable
    {
        int SubscriptionsCount { get; }

        void Subscribe(RasNotificationContext context);
        void Reset();
    }
}