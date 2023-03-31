namespace Tiny.Shared.Exceptions;

public abstract class DomainException : Exception
{
    public abstract string DomainName { get; }

    protected DomainException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
