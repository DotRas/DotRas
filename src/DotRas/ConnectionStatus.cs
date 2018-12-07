namespace DotRas
{
    public class ConnectionStatus
    {
        public virtual ConnectionState State { get; }
        public virtual Device Device { get; }
        public virtual string PhoneNumber { get; }

        public ConnectionStatus(ConnectionState state, Device device, string phoneNumber)
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