using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class WorkspaceErrors
{
    public static Error NameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Workspace), nameof(Workspace.Name));
    public static Error NameTooLong(string actualValue) =>
        StringErrors.TooLong(nameof(Workspace), nameof(Workspace.Name), WorkspaceName.MaxLength, actualValue);
    
    public static Error NotFoundById(Guid id) => CommonErrors.NotFoundById(nameof(Workspace), id);
}