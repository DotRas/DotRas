namespace DotRas.Devices
{
    /// <summary>
    /// Represents a Packet Assembler/Disassembler (PAD) device.
    /// </summary>
    public class Pad : RasDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pad"/> class.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        public Pad(string name) 
            : base(name)
        {
        }
    }
}