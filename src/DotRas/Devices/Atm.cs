namespace DotRas.Devices
{
    /// <summary>
    /// Represents an Asynchronous Transfer Mode (ATM) device.
    /// </summary>
    public class Atm : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Atm"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Atm(string name) 
            : base(name)
        {
        }
    }
}