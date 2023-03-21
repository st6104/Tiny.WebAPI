using Tiny.Application.ViewModelExtensions;
using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

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
        return await _repository.GetOneAsync(request.Id, cancellationToken).ToViewModel();
    }
}
