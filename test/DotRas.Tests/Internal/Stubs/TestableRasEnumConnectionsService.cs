using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Interop;
using DotRas.Internal.Services.Connections;

namespace DotRas.Tests.Internal.Stubs
{
    internal class TestableRasEnumConnectionsService : RasEnumConnectionsService
    {
        public Func<IntPtr, RasHandle> OnCreateHandleFromPtrCallback { get; set; }
        
        public TestableRasEnumConnectionsService(IRasApi32 api, IDeviceTypeFactory deviceTypeFactory, IExceptionPolicy exceptionPolicy, IStructArrayFactory structFactory, IServiceProvider serviceLocator) 
            : base(api, deviceTypeFactory, exceptionPolicy, structFactory, serviceLocator)
        {
        }

        protected override RasHandle CreateHandleFromPtr(IntPtr hRasConn)
        {
            return OnCreateHandleFromPtrCallback?.Invoke(hRasConn);
        }
    }
}