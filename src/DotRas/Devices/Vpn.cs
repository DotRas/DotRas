namespace DotRas.Devices
{
    /// <summary>
    /// Represents a Virtual Private Network (VPN) device.
    /// </summary>
    public class Vpn : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vpn"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Vpn(string name) 
            : base(name)
        {
        }
    }
}