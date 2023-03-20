using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Api.Exceptions;

public class DbConnectionStringErrorException : Exception
{
    public DbConnectionStringErrorException() : base()
    {
    }

    public DbConnectionStringErrorException(string? message) : base(message)
    {
    }

    public DbConnectionStringErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
