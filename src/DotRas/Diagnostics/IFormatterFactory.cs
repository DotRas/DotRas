namespace DotRas.Diagnostics
{
    /// <summary>
    /// Identifies a factory of formatters.
    /// </summary>
    public interface IFormatterFactory
    {
        /// <summary>
        /// Creates a formatter for the type.
        /// </summary>
        /// <typeparam name="T">The type of object to be formatted.</typeparam>
        /// <returns>The formatter instance.</returns>
        IFormatter<T> Create<T>();
    }
}