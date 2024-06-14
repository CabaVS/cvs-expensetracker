namespace CabaVS.ExpenseTracker.Application.Common.Abstractions;

public interface IWorkspaceAdminBoundedRequest
{
    public Guid WorkspaceId { get; }
}