using FluentValidation;
using Tiny.Application.Handlers.Commands;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.Validations;

public class GLAccountAddCommandValidator : AbstractValidator<GLAccountAddCommand>
{
    public GLAccountAddCommandValidator()
    {//Validator에서는 객체에 대한 간단한 필드유효성 검사만 하고 복잡한 비즈니스 로직이나 Db에 엑세스가 필요한 로직은 핸들러나 비즈니스 서비스에서 하도록 하자.
        RuleFor(x => x.Code).NotEmpty().MaximumLength(GLAccount.CodeLength); //.MustAsync(NotExistingCodeAsync).WithMessage("이미 존재하는 계정과목코드입니다.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(GLAccount.NameLength);
        RuleFor(x => x.PostableId);//.MustExistingPostableId(_poastableService);
        RuleFor(x => x.AccountTypeId);//.MustExistingAccountingTypeId(_accountingTypeService);
    }

    // private async Task<bool> NotExistingCodeAsync(string code, CancellationToken cancellationToken)
    // {
    //     return !await _glAccountService.IsExistingCode(code, cancellationToken);
    // }
}

// internal static class GLAccountValidatorExtension
// {
//     public static IRuleBuilderOptions<T, int> MustExistingAccountingTypeId<T>(this IRuleBuilder<T, int> ruleBuilder, IAccountingTypeService service)
//     {
//         return ruleBuilder.MustAsync(service.ExistsAsync).WithMessage("유효하지 않은 계정체계입니다.");
//     }

//     public static IRuleBuilderOptions<T, int> MustExistingPostableId<T>(this IRuleBuilder<T, int> ruleBuilder, IPostableService service)
//     {
//         return ruleBuilder.MustAsync(service.ExistsAsync).WithMessage("유효하지 않은 계정유형입니다.");
//     }
// }
