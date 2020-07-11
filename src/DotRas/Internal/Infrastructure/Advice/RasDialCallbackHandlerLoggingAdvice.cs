using System;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Internal.Abstractions.Services;

namespace DotRas.Internal.Infrastructure.Advice
{
    internal class RasDialCallbackHandlerLoggingAdvice : LoggingAdvice<IRasDialCallbackHandler>, IRasDialCallbackHandler
    {
        public RasDialCallbackHandlerLoggingAdvice(IRasDialCallbackHandler attachedObject, IEventLoggingPolicy eventLoggingPolicy)
            : base(attachedObject, eventLoggingPolicy)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AttachedObject.Dispose();
            }

            base.Dispose(disposing);
        }

        public void Initialize(TaskCompletionSource<RasConnection> completionSource, Action<StateChangedEventArgs> onStateChangedCallback, Action onCompletedCallback, CancellationToken cancellationToken)
        {
            AttachedObject.Initialize(completionSource, onStateChangedCallback, onCompletedCallback, cancellationToken);
        }

        public bool OnCallback(IntPtr dwCallbackId, int dwSubEntry, IntPtr hrasconn, uint message, RasConnectionState rascs, int dwError, int dwExtendedError)
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

        public void SetHandle(IntPtr handle)
        {
            AttachedObject.SetHandle(handle);
        }
    }
}