namespace DotRas.Devices
{
    /// <summary>
    /// Represents an unknown device.
    /// </summary>
    /// <remarks>This device is only used when an unknown device is returned from the operating system.</remarks>
    public class Unknown : RasDevice
    {
        /// <summary>
        /// Gets the device type.
        /// </summary>
        public string DeviceType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unknown"/> class.
        /// </summary>
        /// <param name="name">The device name.</param>
        /// <param name="deviceType">The device type.</param>
        public Unknown(string name, string deviceType) 
            : base(name)
        {
            DeviceType = deviceType;
        }
    }
}