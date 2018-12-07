namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IStructFactory
    {
        T Create<T>() where T : new();
        T Create<T>(out int size) where T : new();
    }
}