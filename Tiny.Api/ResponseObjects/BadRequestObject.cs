// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FluentValidation.Results;

namespace Tiny.Api.ResponseObjects;

public class BadRequestObject
{
    private const string StandardMessage = "유효성 검증오류가 발생하였습니다.";
    public string Message { get; } = StandardMessage;

    public IReadOnlyDictionary<string, IReadOnlyList<string>> Errors => _errors;
    private readonly Dictionary<string, IReadOnlyList<string>> _errors = new();

    public BadRequestObject(IEnumerable<ValidationFailure> failures)
    {
        var failuresByPropertyName = failures.GroupBy(failure => failure.PropertyName).Select(failure =>
            new { PropertyName = failure.Key, Errors = failure.Select(x => x.ErrorMessage) });

        foreach (var failure in failuresByPropertyName)
        {
            _errors.Add(failure.PropertyName, failure.Errors.ToList());
        }
    }

    public BadRequestObject(string identifier, string message)
    {
        _errors.Add(identifier, new[] { message });
    }

    public BadRequestObject(string message)
    {
        Message = message;
    }
}
