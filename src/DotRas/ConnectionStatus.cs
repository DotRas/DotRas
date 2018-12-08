namespace DotRas
{
    public class ConnectionStatus
    {
        public virtual RasConnectionState State { get; }
        public virtual Device Device { get; }
        public virtual string PhoneNumber { get; }

        public ConnectionStatus(RasConnectionState state, Device device, string phoneNumber)
        {
            State = state;
            Device = device;
            PhoneNumber = phoneNumber;
        }

        protected ConnectionStatus()
        {
        }
    }
}