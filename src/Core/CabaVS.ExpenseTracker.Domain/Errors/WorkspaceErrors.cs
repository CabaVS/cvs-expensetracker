using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class WorkspaceErrors
{
    public static Error WorkspaceNameIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(Workspace), nameof(Workspace.Name));
    
    public static Error WorkspaceNameIsTooLong(string actualValue) =>
        StringErrors.IsTooLong(nameof(Workspace), nameof(Workspace.Name), WorkspaceName.MaxLength, actualValue);
}