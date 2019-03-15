using System;
using System.Diagnostics;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.ExternDll;

namespace DotRas.Internal.Infrastructure.Advice
{
    internal class Kernel32LoggingAdvice : LoggingAdvice<IKernel32>, IKernel32
    {
        public Kernel32LoggingAdvice(IKernel32 attachedObject, IEventLoggingPolicy eventLoggingPolicy) 
            : base(attachedObject, eventLoggingPolicy)
        {
        }

        public int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, ref IntPtr lpBuffer, int nSize, IntPtr arguments)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = AttachedObject.FormatMessage(dwFlags, lpSource, dwMessageId, dwLanguageId, ref lpBuffer, nSize, arguments);
            stopwatch.Stop();

            var callEvent = new PInvokeInt32CallCompletedTraceEvent
            {
                DllName = Kernel32Dll,
                Duration = stopwatch.Elapsed,
                MethodName = nameof(FormatMessage),
                Result = result
            };

            callEvent.Args.Add(nameof(dwFlags), dwFlags);
            callEvent.Args.Add(nameof(lpSource), lpSource);
            callEvent.Args.Add(nameof(dwMessageId), dwMessageId);
            callEvent.Args.Add(nameof(dwLanguageId), dwLanguageId);
            callEvent.Args.Add(nameof(nSize), nSize);
            callEvent.Args.Add(nameof(arguments), arguments);

            callEvent.OutArgs.Add(nameof(lpBuffer), lpBuffer);

            LogVerbose(callEvent);
            return result;
        }
    }
}