namespace DotRas.Diagnostics.Tracing
{
    internal interface IEventFormatterAdapter
    {
        string Format(object value);
    }
}