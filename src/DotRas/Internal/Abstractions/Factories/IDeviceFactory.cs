namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IDeviceFactory
    {
        RasDevice Create(string name);
    }
}