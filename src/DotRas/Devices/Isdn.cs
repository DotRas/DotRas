namespace DotRas.Devices
{
    /// <summary>
    /// Represents an Integrated Service Digital Network (ISDN) device.
    /// </summary>
    public class Isdn : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Isdn"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Isdn(string name) 
            : base(name)
        {
        }
    }
}