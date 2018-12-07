namespace DotRas.Devices
{
    /// <summary>
    /// Represents a Serial device.
    /// </summary>
    public class Serial : Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Serial"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Serial(string name) 
            : base(name)
        {
        }
    }
}