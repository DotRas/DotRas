using System;
using System.Collections.Generic;
using System.Linq;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Primitives;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Infrastructure.Primitives;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.NativeMethods;
using static DotRas.Internal.Interop.Ras;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasConnectionNotificationService : DisposableObject, IRasConnectionNotification
    {
        private readonly IRasApi32 api;
        private readonly IRasConnectionNotificationCallbackHandler callbackHandler;
        private readonly IExceptionPolicy exceptionPolicy;

        private readonly IDictionary<RASCN, RasConnectionNotificationStateObject> registrations = 
            new Dictionary<RASCN, RasConnectionNotificationStateObject>();

        public RasConnectionNotificationService(IRasApi32 api, IRasConnectionNotificationCallbackHandler callbackHandler, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.callbackHandler = callbackHandler ?? throw new ArgumentNullException(nameof(callbackHandler));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public void Subscribe(RasNotificationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            callbackHandler.Initialize();

            var handle = DetermineHandleForSubscribe(context);
            if (ShouldRegisterForConnectedEvents(handle))
            {
                RegisterCallback(handle, context.OnConnectedCallback, RASCN.Connection);
            }

            RegisterCallback(handle, context.OnDisconnectedCallback, RASCN.Disconnection);
        }

        private IntPtr DetermineHandleForSubscribe(RasNotificationContext context)
        {
            return context.Connection?.Handle ?? INVALID_HANDLE_VALUE;
        }

        private bool ShouldRegisterForConnectedEvents(IntPtr handle)
        {
            return handle == INVALID_HANDLE_VALUE;
        }

        private void RegisterCallback(IntPtr handle, Action<RasConnectionEventArgs> callback, RASCN changeNotification)
        {
            var stateObject = new RasConnectionNotificationStateObject();
            IRegisteredCallback registeredCallback = null;

            try
            {
                registeredCallback = CreateRegisteredCallback(stateObject);

                var ret = api.RasConnectionNotification(handle, registeredCallback.Handle, changeNotification);
                if (ret != SUCCESS)
                {
                    throw exceptionPolicy.Create(ret);
                }

                stateObject.RegisteredCallback = registeredCallback;
                stateObject.Callback = callback;
                stateObject.NotificationType = changeNotification;

                registrations.Add(changeNotification, stateObject);
            }
            catch (Exception)
            {
                registeredCallback?.Dispose();
                throw;
            }
        }

        protected virtual IRegisteredCallback CreateRegisteredCallback(object state)
        {
            return RegisteredCallback.Create(callbackHandler.OnCallback, state);
        }
    }
}