using Tiny.Application.ViewModelExtensions;
using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.Handlers.Commands;

public record GLAccountUpdateCommand(long Id, string Code, string Name, decimal Balance) : IRequest<GLAccountViewModel>;

public class GLAcountUpdateCommandHandler : IRequestHandler<GLAccountUpdateCommand, GLAccountViewModel>
{
   private readonly IGLAccountRepository _repository;

    public GLAcountUpdateCommandHandler(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<GLAccountViewModel> Handle(GLAccountUpdateCommand request, CancellationToken cancellationToken)
    {
        var glAccount = await _repository.GetOneAsync(request.Id, cancellationToken);
        glAccount.ChangeCode(request.Code)
                .ChangeName(request.Name)
                .ChangeBalance(request.Balance);

        return await _repository.UpdateAsync(glAccount, cancellationToken).ToViewModel();
    }
}