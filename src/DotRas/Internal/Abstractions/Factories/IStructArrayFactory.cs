namespace DotRas.Internal.Abstractions.Factories
{
    internal interface IStructArrayFactory
    {
        T[] CreateArray<T>(int count, out int size) where T : new();
    }
}