namespace DotRas.Devices
{
    /// <summary>
    /// Represents a generic device.
    /// </summary>
    public class Generic : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Generic"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Generic(string name)
            : base(name)
        {
        }
    }
}