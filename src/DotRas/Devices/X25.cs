namespace DotRas.Devices
{
    /// <summary>
    /// Represents an X.25 device.
    /// </summary>
    public class X25 : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="X25"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public X25(string name) 
            : base(name)
        {
        }
    }
}