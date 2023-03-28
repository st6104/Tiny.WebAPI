using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Shared.Exceptions;

namespace Tiny.Domain.Exceptions;

public sealed class GLAccountValidationError : DomainValidationErrorException
{
    public override string DomainName => nameof(GLAccount);

    public GLAccountValidationError(string identifier, string message, Exception? innerException = null) : base(identifier, message, innerException)
    {
    }

    private GLAccountValidationError() : base()
    {
    }

    private GLAccountValidationError(string? message) : base(message)
    {
    }

    private GLAccountValidationError(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
