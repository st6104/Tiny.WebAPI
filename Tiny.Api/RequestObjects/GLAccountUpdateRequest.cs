using Tiny.Application.Handlers.Commands;

namespace Tiny.Api.RequestObjects;

public record GLAccountUpdateRequest(string Code, string Name, decimal Balance);

internal static class GLAcountUpdateRequestExtensions
{
    public static GLAccountUpdateCommand ToCommand(this GLAccountUpdateRequest request, long id)
    {
        return new GLAccountUpdateCommand(id, request.Code, request.Name, request.Balance);
    }
}
