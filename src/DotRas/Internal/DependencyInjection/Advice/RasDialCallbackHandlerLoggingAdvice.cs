using System;
using System.Threading;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Abstractions.Threading;
using DotRas.Win32.SafeHandles;

namespace DotRas.Internal.DependencyInjection.Advice
{
    internal class RasDialCallbackHandlerLoggingAdvice : LoggingAdvice<IRasDialCallbackHandler>, IRasDialCallbackHandler
    {
        public RasDialCallbackHandlerLoggingAdvice(IRasDialCallbackHandler attachedObject, IEventLoggingPolicy eventLoggingPolicy)
            : base(attachedObject, eventLoggingPolicy)
        {
        }

        public void Initialize(ITaskCompletionSource<Connection> completionSource, Action<DialerStateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken)
        {
            AttachedObject.Initialize(completionSource, onStateChangedCallback, onCompletedCallback, cancellationToken);
        }

        public bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hrasconn, uint message, ConnectionState rascs, int dwError, int dwExtendedError)
        {
            var occurredOn = DateTime.Now;
            var result = AttachedObject.OnCallback(dwCallbackId, dwSubEntry, hrasconn, message, rascs, dwError, dwExtendedError);

            var callbackEvent = new RasDialCallbackCompletedTraceEvent
            {
                OccurredOn = occurredOn,
                Result = result
            };

            callbackEvent.Args.Add(nameof(dwCallbackId), dwCallbackId);
            callbackEvent.Args.Add(nameof(dwSubEntry), dwSubEntry);
            callbackEvent.Args.Add(nameof(hrasconn), hrasconn);
            callbackEvent.Args.Add(nameof(message), message);
            callbackEvent.Args.Add(nameof(rascs), rascs);
            callbackEvent.Args.Add(nameof(dwError), dwError);
            callbackEvent.Args.Add(nameof(dwExtendedError), dwExtendedError);

            LogVerbose(callbackEvent);
            return result;
        }

        public void SetHandle(RasHandle value)
        {
            AttachedObject.SetHandle(value);
        }
    }
}