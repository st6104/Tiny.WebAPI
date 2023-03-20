namespace Tiny.Infrastructure.Exceptions;

public class TransactionAlreadyExistsException : Exception
{
    public TransactionAlreadyExistsException() : base()
    {
    }

    public TransactionAlreadyExistsException(string? message) : base(message)
    {
    }

    public TransactionAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}