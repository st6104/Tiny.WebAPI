using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Tiny.Api.Exceptions;

public sealed class ControllerResultException : Exception
{
    public HttpStatusCode StatusCode{ get; }

    public ControllerResultException(string? message, Exception? innerException, HttpStatusCode statusCode) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public ControllerResultException() : base()
    {
    }

    public ControllerResultException(string? message) : base(message)
    {
    }

    public ControllerResultException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
