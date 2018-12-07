namespace DotRas
{
    internal interface IConverter<in TInput, out TResult>
    {
        TResult Convert(TInput input);
    }
}