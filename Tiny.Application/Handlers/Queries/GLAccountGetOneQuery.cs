using Tiny.Application.ViewModelExtensions;
using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Specifications;

namespace Tiny.Application.Handlers.Queries;

public record GLAccountGetOneQuery(long Id) : IRequest<GLAccountViewModel>;

public class GLAccountGetOneQueryHandler : IRequestHandler<GLAccountGetOneQuery, GLAccountViewModel>
{
    private readonly IGLAccountRepository _repository;

    public GLAccountGetOneQueryHandler(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<GLAccountViewModel> Handle(GLAccountGetOneQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetOneAsync(new GLAccountByIdSpec(request.Id), cancellationToken).ToViewModelAsync();
    }
}
