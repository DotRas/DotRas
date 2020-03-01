using System.Collections.Generic;
using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;

namespace DotRas
{
    /// <summary>
    /// Represents a device capable of establishing a remote access connection.
    /// </summary>
    public abstract class RasDevice
    {
        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasDevice"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        protected RasDevice(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasDevice"/> class.
        /// </summary>
        protected RasDevice()
        {
        }

        /// <summary>
        /// Enumerates the devices.
        /// </summary>
        /// <returns>An enumerable used to iterate through the devices available.</returns>
        public static IEnumerable<RasDevice> EnumerateDevices()
        {
            return ServiceLocator.Default.GetRequiredService<IRasEnumDevices>()
                .EnumerateDevices();
        }
    }
}