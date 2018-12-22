using System;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using static DotRas.Internal.Interop.NativeMethods;

namespace DotRas.Internal.Services.Dialing
{
    internal class RasDialExtensionsBuilder : IRasDialExtensionsBuilder
    {
        private readonly IStructFactory structFactory;

        public RasDialExtensionsBuilder(IStructFactory structFactory)
        {
            this.structFactory = structFactory ?? throw new ArgumentNullException(nameof(structFactory));
        }

        public RASDIALEXTENSIONS Build(RasDialContext context)
        {
            return structFactory.Create<RASDIALEXTENSIONS>();
        }
    }
}