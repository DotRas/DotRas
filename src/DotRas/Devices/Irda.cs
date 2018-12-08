namespace DotRas.Devices
{
    /// <summary>
    /// Represents an Infrared Data Association (IrDA) compliant device.
    /// </summary>
    public class Irda : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Irda"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Irda(string name) 
            : base(name)
        {
        }
    }
}