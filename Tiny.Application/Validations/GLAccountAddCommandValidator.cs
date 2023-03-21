using System.Security.Cryptography;
using FluentValidation;
using Tiny.Application.Handlers.Commands;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

namespace Tiny.Application.Validations;

public class GLAccountAddCommandValidator : AbstractValidator<GLAccountAddCommand>
{
    private readonly IAccountingTypeService _accountingTypeService;
    private readonly IPostableService _poastableService;
    private readonly IGLAccountService _glAccountService;

    public GLAccountAddCommandValidator(IAccountingTypeService accountingTypeService, IPostableService poastableService, IGLAccountService glAccountService)
    {
        _glAccountService = glAccountService;
        _accountingTypeService = accountingTypeService;
        _poastableService = poastableService;

        RuleFor(x => x.Code).NotEmpty().MustAsync(NotExistingCodeAsync).WithMessage("이미 존재하는 계정과목코드입니다.");
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.PostableId).MustExistingPostableId(_poastableService);
        RuleFor(x => x.AccountTypeId).MustExistingAccountingTypeId(_accountingTypeService);
    }

    private async Task<bool> NotExistingCodeAsync(string code, CancellationToken cancellationToken)
    {
        return !await _glAccountService.IsExistingCode(code, cancellationToken);
    }
}

internal static class GLAccountValidatorExtension
{
    public static IRuleBuilderOptions<T, int> MustExistingAccountingTypeId<T>(this IRuleBuilder<T, int> ruleBuilder, IAccountingTypeService service)
    {
        return ruleBuilder.MustAsync(service.ExistsAsync).WithMessage("유효하지 않은 계정체계입니다.");
    }

    public static IRuleBuilderOptions<T, int> MustExistingPostableId<T>(this IRuleBuilder<T, int> ruleBuilder, IPostableService service)
    {
        return ruleBuilder.MustAsync(service.ExistsAsync).WithMessage("유효하지 않은 계정유형입니다.");
    }
}