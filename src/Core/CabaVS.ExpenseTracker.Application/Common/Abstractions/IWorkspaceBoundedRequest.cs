namespace CabaVS.ExpenseTracker.Application.Common.Abstractions;

internal interface IWorkspaceBoundedRequest
{
    public Guid WorkspaceId { get; }
}