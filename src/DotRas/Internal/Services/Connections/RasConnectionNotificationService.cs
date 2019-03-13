using System;
using System.Collections.Generic;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasConnectionNotificationService : DisposableObject, IRasConnectionNotification
    {
        private readonly IRasApi32 rasApi32;
        private readonly IRasConnectionNotificationCallbackHandler callbackHandler;
        private readonly IExceptionPolicy exceptionPolicy;
        private readonly IRegisteredCallbackFactory callbackFactory;

        private readonly IDictionary<RASCN, RasConnectionNotificationStateObject> notifications = 
            new Dictionary<RASCN, RasConnectionNotificationStateObject>();

        public RasConnectionNotificationService(IRasApi32 rasApi32, IRasConnectionNotificationCallbackHandler callbackHandler, IExceptionPolicy exceptionPolicy, IRegisteredCallbackFactory callbackFactory)
        {
            this.rasApi32 = rasApi32 ?? throw new ArgumentNullException(nameof(rasApi32));
            this.callbackHandler = callbackHandler ?? throw new ArgumentNullException(nameof(callbackHandler));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
            this.callbackFactory = callbackFactory ?? throw new ArgumentNullException(nameof(callbackFactory));
        }

        public bool IsActive
        {
            get
            {
                lock (SyncRoot)
                {
                    return notifications.Count > 0;
                }
            }
        }       
        
        public void Subscribe(RasNotificationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            GuardMustNotBeDisposed();

            lock (SyncRoot)
            {
                callbackHandler.Initialize();

                // Connection events always needs an invalid handle.
                RegisterCallback(INVALID_HANDLE_VALUE, context.OnConnectedCallback, RASCN.Connection);

                var handle = DetermineHandleForSubscribe(context);
                RegisterCallback(handle, context.OnDisconnectedCallback, RASCN.Disconnection);
            }
        }

        public void Reset()
        {
            GuardMustNotBeDisposed();

            Unsubscribe();
        }

        private IntPtr DetermineHandleForSubscribe(RasNotificationContext context)
        {
            return context.Connection?.Handle ?? INVALID_HANDLE_VALUE;
        }

        private void RegisterCallback(IntPtr handle, Action<RasConnectionEventArgs> callback, RASCN changeNotification)
        {
            var stateObject = new RasConnectionNotificationStateObject();
            IRegisteredCallback registeredCallback = null;

            try
            {
                registeredCallback = callbackFactory.Create(callbackHandler.OnCallback, stateObject);
                if (registeredCallback == null)
                {
                    throw new InvalidOperationException("The factory did not register a callback.");
                }

                var ret = rasApi32.RasConnectionNotification(handle, registeredCallback.Handle, changeNotification);
                if (ret != SUCCESS)
                {
                    throw exceptionPolicy.Create(ret);
                }

                stateObject.RegisteredCallback = registeredCallback;
                stateObject.Callback = callback;
                stateObject.NotificationType = changeNotification;

                notifications.Add(changeNotification, stateObject);
            }
            catch (Exception)
            {
                registeredCallback?.Dispose();
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unsubscribe();
            }

            base.Dispose(disposing);
        }

        private void Unsubscribe()
        {
            lock (SyncRoot)
            {
                foreach (var subscription in notifications)
                {
                    subscription.Value.RegisteredCallback.Dispose();
                }

                notifications.Clear();
            }
        }
    }
}