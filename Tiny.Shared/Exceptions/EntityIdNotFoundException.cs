using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tiny.Shared.Exceptions;

/// <summary>
/// 엔티티id를 찾을수 없을때 발생하는 예외
/// </summary>
public sealed class EntityIdNotFoundException : Exception
{
    public long FailedId { get; }

    public EntityIdNotFoundException(long failedId, string? message = null, Exception? innerException = null) : base(message, innerException)
    {
        FailedId = failedId;
    }

    private EntityIdNotFoundException() : base()
    {
    }

    private EntityIdNotFoundException(string? message) : base(message)
    {
    }

    private EntityIdNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
