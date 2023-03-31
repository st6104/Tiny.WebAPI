using Tiny.Application.ViewModels;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

namespace Tiny.Application.ViewModelExtensions;

internal static class DomainToViewModelExtension
{
    public static Task<GLAccountViewModel> ToViewModelAsync(this Task<GLAccount> entityTask)
    {
        return Task.Run(async () =>
        {
            var entity = await entityTask;
            return ToViewModelCore(entity);
        });
    }

    public static async Task<IEnumerable<GLAccountViewModel>> ToViewModelAsync(this Task<IReadOnlyList<GLAccount>> entitiesTask)
    {
        var entities = await entitiesTask;
        return entities.Select(ToViewModelCore);
    }

    public static GLAccountViewModel ToViewModel(this GLAccount entity)
    {
        return ToViewModelCore(entity);
    }

    private static GLAccountViewModel ToViewModelCore(GLAccount entity)
    {
        return new GLAccountViewModel(entity.Id, entity.Code, entity.Name, entity.PostableId, entity.AccountingTypeId, entity.Balance);
    }
}
