using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Application.Common.Errors;

public static class WorkspaceAccessErrors
{
    public static Error NoAccess(Guid workspaceId) =>
        new(
            "WorkspaceAccess.NoAccess", 
            $"Workspace '{workspaceId}' doesn't exist or Current User has no access over it.");
    public static Error NotAdmin(Guid workspaceId) =>
        new(
            "WorkspaceAccess.NotAdmin", 
            $"Current User is not an admin over Workspace '{workspaceId}'.");
}