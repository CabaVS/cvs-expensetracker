using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class WorkspaceMemberEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    
    public bool IsAdmin { get; set; }

    public Guid UserId { get; set; }
    public UserEf? User { get; set; }

    public Guid WorkspaceId { get; set; }
    public WorkspaceEf? Workspace { get; set; }
    
    internal static WorkspaceMemberEf FromDomain(WorkspaceMember workspaceMember) => new()
    {
        Id = workspaceMember.Id,
        CreatedOn = workspaceMember.CreatedOn,
        ModifiedOn = workspaceMember.ModifiedOn,
        IsAdmin = workspaceMember.IsAdmin,
        UserId = workspaceMember.User.Id
    };
}
