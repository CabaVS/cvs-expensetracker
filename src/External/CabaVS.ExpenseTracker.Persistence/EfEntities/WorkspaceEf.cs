namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class WorkspaceEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<WorkspaceMemberEf> Members { get; set; } = [];
}
