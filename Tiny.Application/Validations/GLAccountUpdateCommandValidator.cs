using System.Data;
using FluentValidation;
using Tiny.Application.Handlers.Commands;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

namespace Tiny.Application.Validations;

public class GLAccountUpdateCommandValidator : AbstractValidator<GLAccountUpdateCommand>
{
    private readonly IGLAccountService _glAcountService;

    public GLAccountUpdateCommandValidator(IGLAccountService glAcountService)
    {
        _glAcountService = glAcountService;

        RuleFor(x => x.Code).NotEmpty().MustAsync(NotExistingCode).WithMessage("이미 존재하는 계정코드입니다.");
        RuleFor(x => x.Name).NotEmpty();
        //RuleFor(x => x.Balance);
    }

    private async Task<bool> NotExistingCode(GLAccountUpdateCommand command, string code, CancellationToken cancellationToken)
    {
        return !await _glAcountService.IsExistingCode(command.Id, code, cancellationToken);
    }
}
