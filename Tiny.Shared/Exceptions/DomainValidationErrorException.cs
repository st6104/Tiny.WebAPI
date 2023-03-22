using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Shared.Exceptions;

public abstract class DomainValidationErrorException : DomainException
{
    public string Identifier { get; } = string.Empty;

    protected DomainValidationErrorException() : base()
    {
    }

    protected DomainValidationErrorException(string? message) : base(message)
    {
    }

    protected DomainValidationErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DomainValidationErrorException(string identifier, string message, Exception? innerException = null) : base(message, innerException)
    {
        Identifier = identifier;
    }
}
