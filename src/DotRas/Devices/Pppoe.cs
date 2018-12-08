namespace DotRas.Devices
{
    /// <summary>
    /// Represents a Point-to-Point Protocol over Ethernet (PPPoE) device.
    /// </summary>
    public class Pppoe : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pppoe"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Pppoe(string name) 
            : base(name)
        {
        }
    }
}