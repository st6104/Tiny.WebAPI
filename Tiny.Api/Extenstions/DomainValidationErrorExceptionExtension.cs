using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using Tiny.Shared.Exceptions;

namespace Tiny.Api.Extenstions;

internal static class DomainValidationErrorExceptionExtension
{
    public static IEnumerable<ValidationError> ToValidationErrors(this DomainValidationErrorException exception)
    {
        var validationError = new ValidationError
        {
            Identifier = exception.Identifier,
            ErrorMessage = exception.Message,
            Severity = ValidationSeverity.Error
        };

        return new ValidationError[] { validationError };
    }
}
