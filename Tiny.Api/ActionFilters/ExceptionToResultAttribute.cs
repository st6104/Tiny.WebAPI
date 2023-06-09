using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tiny.Api.Extenstions;
using Tiny.Shared.Exceptions;

namespace Tiny.Api.ActionFilters;

[Obsolete("이제 사용하지 않음", true)]
public class ExceptionToResultAttribute : TranslateResultToActionResultAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not null && context.Controller is ControllerBase controllerBase)
        {
            if (!TryConvertToResult(context.Exception, out var result))
                return;

            context.ExceptionHandled = true;
            context.Result = controllerBase.ToActionResult(result);
        }

        base.OnActionExecuted(context);
    }

    private static bool TryConvertToResult(Exception ex, out Result? result)
    {
        result = null;

        if (ex is EntityIdNotFoundException)
            result = Result.NotFound();
        else if (ex is ValidationException validationException)
            result = Result.Invalid(validationException.Errors.ToValidationErrors().ToList());
        else if(ex is DomainValidationErrorException domainValidationErrorException)
            result = Result.Invalid(domainValidationErrorException.ToValidationErrors().ToList());

        return result is not null;
    }
}

internal static class ValidationFailureExtension
{
    public static IReadOnlyList<ValidationError> ToValidationErrors(this IEnumerable<ValidationFailure> failures)
    {
        return failures.Select(f => new ValidationError
        {
            Identifier = f.PropertyName,
            ErrorMessage = f.ErrorMessage,
            ErrorCode = f.ErrorCode,
            Severity = FluentValidationResultExtensions.FromSeverity(f.Severity),
        }).ToList().AsReadOnly();
    }
}
