using FluentValidation;
using Microsoft.Extensions.Logging;
using Tiny.Shared.Extensions;

namespace Tiny.Application.Bahaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("----- Validating command {CommandType}", typeName);

        var failuresTasks = _validators.Select(v => v.ValidateAsync(request, cancellationToken));
        var failures = (await Task.WhenAll(failuresTasks)).SelectMany(result => result.Errors).Where(error => error is not null).ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);
            throw new ValidationException("Validation Error Occurred", failures);
        }

        return await next();
    }
}
