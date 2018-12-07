namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IDeviceFactory<out T>
        where T : Device
    {
        T Create(string name);
    }
}