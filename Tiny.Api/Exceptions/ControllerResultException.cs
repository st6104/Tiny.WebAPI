using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Tiny.Api.Exceptions;

public sealed class ControllerResultException : Exception
{
    public HttpStatusCode StatusCode{ get; }

    public ControllerResultException(string? message, Exception? innerException, HttpStatusCode statusCode) : this(message, innerException)
    {
        StatusCode = statusCode;
    }

    private ControllerResultException() : base()
    {
    }

    private ControllerResultException(string? message) : base(message)
    {
    }

    private ControllerResultException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
