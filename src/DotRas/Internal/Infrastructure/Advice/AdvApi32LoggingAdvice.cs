using System.Diagnostics;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.ExternDll;

namespace DotRas.Internal.Infrastructure.Advice
{
    internal class AdvApi32LoggingAdvice : LoggingAdvice<IAdvApi32>, IAdvApi32
    {
        public AdvApi32LoggingAdvice(IAdvApi32 attachedObject, IEventLoggingPolicy eventLoggingPolicy)
            : base(attachedObject, eventLoggingPolicy)
        {
        }

        public bool AllocateLocallyUniqueId(out Luid luid)
        {            
            var stopwatch = Stopwatch.StartNew();
            var result = AttachedObject.AllocateLocallyUniqueId(out luid);
            stopwatch.Stop();

            var callEvent = new PInvokeBoolCallCompletedTraceEvent
            {
                DllName = AdvApi32Dll,
                Duration = stopwatch.Elapsed,
                MethodName = nameof(AllocateLocallyUniqueId),
                Result = result
            };

            callEvent.OutArgs.Add(nameof(luid), luid);
            LogVerbose(callEvent);

            return result;
        }
    }
}