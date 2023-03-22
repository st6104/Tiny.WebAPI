using Tiny.Application.ViewModelExtensions;
using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.Handlers.Queries;

public record GLAccountGetManyQuery : IRequest<IReadOnlyList<GLAccountViewModel>>;

public class GLAccountGetManyQueryHandler : IRequestHandler<GLAccountGetManyQuery, IReadOnlyList<GLAccountViewModel>>
{
    private readonly IGLAccountRepository _repository;

    public GLAccountGetManyQueryHandler(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<GLAccountViewModel>> Handle(GLAccountGetManyQuery request, CancellationToken cancellationToken)
    {
        return (await _repository.GetAllAsync(cancellationToken).ToViewModelAsync()).ToList().AsReadOnly();
    }
}