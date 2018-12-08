namespace DotRas.Devices
{
    /// <summary>
    /// Represents a direct connection through a Parallel port.
    /// </summary>
    public class Parallel : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parallel"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Parallel(string name) 
            : base(name)
        {
        }
    }
}