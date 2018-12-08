namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IDeviceTypeFactory
    {
        RasDevice Create(string name, string deviceType);
    }
}