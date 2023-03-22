using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Shared.Exceptions;

namespace Tiny.Domain.Exceptions;

public class GLAccountValidationError : DomainValidationErrorException
{
    public override string DomainName => nameof(GLAccount);

    protected GLAccountValidationError() : base()
    {
    }

    public GLAccountValidationError(string? message) : base(message)
    {
    }

    protected GLAccountValidationError(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public GLAccountValidationError(string identifier, string message, Exception? innerException = null) : base(identifier, message, innerException)
    {
    }
}
