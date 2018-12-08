namespace DotRas.Devices
{
    /// <summary>
    /// Represents a modem.
    /// </summary>
    public class Modem : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modem"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Modem(string name) 
            : base(name)
        {
        }
    }
}