using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Shared.Exceptions;

public abstract class DomainException : Exception
{
    public abstract string DomainName { get; }

    protected DomainException() : base()
    {
    }

    protected DomainException(string? message) : base(message)
    {
    }

    protected DomainException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
