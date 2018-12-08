namespace DotRas
{
    public class RasConnectionStatus
    {
        public virtual RasConnectionState State { get; }
        public virtual Device Device { get; }
        public virtual string PhoneNumber { get; }

        public RasConnectionStatus(RasConnectionState state, Device device, string phoneNumber)
        {
            State = state;
            Device = device;
            PhoneNumber = phoneNumber;
        }

        protected RasConnectionStatus()
        {
        }
    }
}