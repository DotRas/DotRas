namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IDeviceFactory
    {
        Device Create(string name);
    }
}