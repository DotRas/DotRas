namespace DotRas.Diagnostics.Tracing
{
    internal interface IFormatterAdapter
    {
        string Format(object value);
    }
}