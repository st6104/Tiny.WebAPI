using Tiny.Application.ViewModelExtensions;
using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;

namespace Tiny.Application.Handlers.Commands;

public record GLAccountUpdateCommand(long Id, string Code, string Name, decimal Balance) : IRequest<GLAccountViewModel>;

public class GLAcountUpdateCommandHandler : IRequestHandler<GLAccountUpdateCommand, GLAccountViewModel>
{
    private readonly IGLAccountRepository _repository;
    private readonly IGLAccountService _service;

    public GLAcountUpdateCommandHandler(IGLAccountRepository repository, IGLAccountService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<GLAccountViewModel> Handle(GLAccountUpdateCommand request, CancellationToken cancellationToken)
    {
        var glAccount = await _repository.GetOneAsync(new GLAccountByIdSpec(request.Id), cancellationToken);
        glAccount.ChangeCode(request.Code)
                .ChangeName(request.Name)
                .ChangeBalance(request.Balance);

        await _service.CheckDuplicatedCodeAsync(glAccount.Code, glAccount.Id, cancellationToken);
        _service.CheckValidAccountingType(glAccount);
        _service.CheckValidPostable(glAccount);

        await _repository.UpdateAsync(glAccount, cancellationToken);
        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return glAccount.ToViewModel();
    }
}