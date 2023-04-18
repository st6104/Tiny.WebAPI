namespace Tiny.Application.ViewModels;

public record GLAccountViewModel(long Id, string Code, string Name, int PostableId,
    int AccountingTypeId, decimal Balance);
