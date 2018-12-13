namespace DotRas.Devices
{
    /// <summary>
    /// Represents a generic device.
    /// </summary>
#pragma warning disable CA1724 // Type names should not match namespaces
    public class Generic : RasDevice
#pragma warning restore CA1724 // Type names should not match namespaces
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