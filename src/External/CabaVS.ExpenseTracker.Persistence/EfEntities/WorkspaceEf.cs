using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class WorkspaceEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<WorkspaceMemberEf> Members { get; set; } = [];
    
    internal static WorkspaceEf FromDomain(Workspace workspace) => new()
    {
        Id = workspace.Id,
        CreatedOn = workspace.CreatedOn,
        ModifiedOn = workspace.ModifiedOn,
        Name = workspace.Name.Value,
        Members = [.. workspace.Members.Select(WorkspaceMemberEf.FromDomain)]
    };
}
