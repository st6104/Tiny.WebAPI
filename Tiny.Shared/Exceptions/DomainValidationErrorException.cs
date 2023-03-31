namespace Tiny.Shared.Exceptions;

public abstract class DomainValidationErrorException : DomainException
{
    public string Identifier { get; }

    protected DomainValidationErrorException(string identifier, string message, Exception? innerException = null) : base(message, innerException)
    {
        Identifier = identifier;
    }
}
