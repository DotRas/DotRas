namespace DotRas.Devices
{
    /// <summary>
    /// Represents a Sonet device.
    /// </summary>
    public class Sonet : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sonet"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Sonet(string name) 
            : base(name)
        {
        }
    }
}