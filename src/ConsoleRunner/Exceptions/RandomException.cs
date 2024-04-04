namespace ConsoleRunner.Exceptions;

/// <summary>
/// An exception which occurs randomly.
/// </summary>
[Serializable]
public class RandomException : Exception
{
    public RandomException()
        : this("A random exception occcured.")
    {
    }

    public RandomException(string message) : base(message)
    {
    }

    public RandomException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}