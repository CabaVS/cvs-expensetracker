namespace CabaVS.ExpenseTracker.Application.Common;

public interface IWorkspaceBoundedRequest
{
    Guid WorkspaceId { get; }
}
