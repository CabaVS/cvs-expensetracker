using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Workspace : AuditableEntity
{
    public WorkspaceName Name { get; set; }

    private Workspace(Guid id, DateTime createdOn, DateTime modifiedOn, WorkspaceName name) : base(id, createdOn, modifiedOn) => 
        Name = name;
    
    public static Result<Workspace> Create(string name)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return Create(Guid.NewGuid(), utcNow, utcNow, name);
    }

    public static Result<Workspace> Create(Guid id, DateTime createdOn, DateTime modifiedOn, string name)
    {
        Result<WorkspaceName> workspaceNameResult = WorkspaceName.Create(name);
        if (workspaceNameResult.IsFailure)
        {
            return workspaceNameResult.Error;
        }
        
        var workspace = new Workspace(id, createdOn, modifiedOn, workspaceNameResult.Value);
        return workspace;
    }
}
