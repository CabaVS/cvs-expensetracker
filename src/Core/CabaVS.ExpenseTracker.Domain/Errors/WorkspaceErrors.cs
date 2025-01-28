using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class WorkspaceErrors
{
    public static Error NotFoundById(Guid workspaceId) => CommonErrors.NotFoundById(nameof(Workspace), workspaceId);
    
    public static Error NameIsNullOrWhitespace() => StringErrors.IsNullOrWhiteSpace(nameof(Workspace), nameof(Workspace.Name));
    public static Error NameIsTooLong(string actual) => StringErrors.IsTooLong(nameof(Workspace), nameof(Workspace.Name), WorkspaceName.MaxLength, actual);
    
    public static Error UserIsNotAnAdminOverWorkspace() =>
        new("Workspace.UserLacksPermissions", "User does not have an admin rights over the workspace."); 
}
