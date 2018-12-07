namespace DotRas
{
    public abstract class Device
    {
        public string Name { get; }

        protected Device(string name)
        {
            Name = name;
        }
    }
}