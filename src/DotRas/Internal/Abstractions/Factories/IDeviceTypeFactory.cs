namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IDeviceTypeFactory
    {
        Device Create(string name, string deviceType);
    }
}