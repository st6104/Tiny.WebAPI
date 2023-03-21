using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Shared.Exceptions;

/// <summary>
/// 엔티티id를 찾을수 없을때 발생하는 예외
/// </summary>
public sealed class IdNotFoundException : Exception
{
    public IdNotFoundException() : base()
    {
    }

    public IdNotFoundException(string? message) : base(message)
    {
    }

    public IdNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
