namespace DotRas.Devices
{
    /// <summary>
    /// Represents a Frame Relay device.
    /// </summary>
    public class FrameRelay : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRelay"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public FrameRelay(string name) 
            : base(name)
        {
        }
    }
}