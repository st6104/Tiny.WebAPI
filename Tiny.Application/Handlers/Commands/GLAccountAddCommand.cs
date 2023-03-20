using System.Net.NetworkInformation;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.Handlers.Commands;

public record GLAccountAddCommand(string Code, string Name, int PostableId, int AccountTypeId) : IRequest<long>;

public class GLAccountAddCommandHandler : IRequestHandler<GLAccountAddCommand, long>
{
    private readonly IGLAccountRepository _repository;

    public GLAccountAddCommandHandler(IGLAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(GLAccountAddCommand request, CancellationToken cancellationToken)
    {
        var glAccount = await _repository.AddAsync(request.ToDomain(), cancellationToken);
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