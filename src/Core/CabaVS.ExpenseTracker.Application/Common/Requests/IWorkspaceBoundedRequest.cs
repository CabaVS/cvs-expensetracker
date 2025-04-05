namespace CabaVS.ExpenseTracker.Application.Common.Requests;

public interface IWorkspaceBoundedRequest
{
    Guid WorkspaceId { get; }
}
