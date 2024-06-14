namespace CabaVS.ExpenseTracker.Application.Common.Abstractions;

internal interface IWorkspaceAdminBoundedRequest
{
    public Guid WorkspaceId { get; }
}