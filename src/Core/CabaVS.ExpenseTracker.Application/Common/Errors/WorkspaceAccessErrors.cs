using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Application.Common.Errors;

public static class WorkspaceAccessErrors
{
    public static Error NoAccess(Guid workspaceId) => WorkspaceErrors.NotFoundById(workspaceId);
    public static Error NotAdmin(Guid workspaceId) =>
        new(
            "WorkspaceAccess.NotAdmin", 
            $"Current User is not an admin over Workspace '{workspaceId}'.");
}