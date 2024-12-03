namespace CabaVS.ExpenseTracker.Application.Common.Requests;

internal interface IWorkspaceBoundRequest
{
    Guid WorkspaceId { get; }
}