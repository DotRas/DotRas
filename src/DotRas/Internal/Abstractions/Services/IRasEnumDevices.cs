namespace DotRas.Internal.Abstractions.Services;

internal interface IRasEnumDevices
{
    IEnumerable<RasDevice> EnumerateDevices();
}