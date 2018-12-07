namespace DotRas.Diagnostics.Tracing
{
    internal interface IFormatter<in T>
    {
        string Format(T eventData);
    }
}