using System.Net.NetworkInformation;
using Tiny.Domain.AggregateModels.GLAccountAggregate;
using Tiny.Domain.AggregateModels.GLAccountAggregate.Services;

namespace Tiny.Application.Handlers.Commands;

public record GLAccountAddCommand(string Code, string Name, int PostableId, int AccountTypeId) : IRequest<long>;

public class GLAccountAddCommandHandler : IRequestHandler<GLAccountAddCommand, long>
{
    private readonly IGLAccountRepository _repository;
    private readonly IGLAccountService _service;

    public GLAccountAddCommandHandler(IGLAccountRepository repository, IGLAccountService service)
    {
        _repository = repository;
        _service = service;
    }

    public async Task<long> Handle(GLAccountAddCommand request, CancellationToken cancellationToken)
    {
        var glAccount = request.ToDomain();
        await _service.CheckDuplicatedCodeAsync(glAccount.Code, cancellationToken);
        _service.CheckValidAccountingType(glAccount);
        _service.CheckValidPostable(glAccount);
        await _repository.AddAsync(glAccount, cancellationToken);

        return glAccount.Id;
    }
}

internal static class GLAccountCommandExtension
{
    public static GLAccount ToDomain(this GLAccountAddCommand command)
    {
        return new GLAccount(command.Code, command.Name, command.PostableId, command.AccountTypeId);
    }
}