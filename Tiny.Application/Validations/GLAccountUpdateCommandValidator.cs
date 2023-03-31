using FluentValidation;
using Tiny.Application.Handlers.Commands;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.Validations;

public class GLAccountUpdateCommandValidator : AbstractValidator<GLAccountUpdateCommand>
{
    public GLAccountUpdateCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(GLAccount.CodeLength);//.MustAsync(NotExistingCode).WithMessage("이미 존재하는 계정코드입니다.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(GLAccount.NameLength);
        //RuleFor(x => x.Balance);
    }

    // private async Task<bool> NotExistingCode(GLAccountUpdateCommand command, string code, CancellationToken cancellationToken)
    // {
    //     return !await _glAcountService.IsExistingCode(command.Id, code, cancellationToken);
    // }
}
