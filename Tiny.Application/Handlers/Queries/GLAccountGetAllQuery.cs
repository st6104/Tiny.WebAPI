using Microsoft.EntityFrameworkCore;
using Tiny.Application.ViewModelExtensions;
using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.Handlers.Queries;

public record GLAccountGetAllQuery : IQueryRequest<IReadOnlyList<GLAccountViewModel>>;

public class
    GLAccountGetAllQueryHandler : IRequestHandler<GLAccountGetAllQuery,
        IReadOnlyList<GLAccountViewModel>>
{
    private readonly IGLAccountRepository _repository;

    public GLAccountGetAllQueryHandler(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<GLAccountViewModel>> Handle(GLAccountGetAllQuery request,
        CancellationToken cancellationToken)
    {
        //TODO : 메모리 캐시를 어디서 쓸까?
        return (await _repository.GetAll(cancellationToken).ToViewModel()
            .ToListAsync(cancellationToken)).AsReadOnly();
    }
}
