using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Workspace : Entity
{
    public WorkspaceName Name { get; private set; }

    private Workspace(
        Guid id, DateTime createdOn, DateTime? modifiedOn,
        WorkspaceName name) : base(id, createdOn, modifiedOn) => Name = name;

    public Result UpdateName(string name, bool isAdminOverWorkspace)
    {
        if (!isAdminOverWorkspace)
        {
            return WorkspaceErrors.AdminRightsRequired(Id);
        }

        Result<WorkspaceName> workspaceNameResult = WorkspaceName.Create(name);
        if (workspaceNameResult.IsFailure)
        {
            return workspaceNameResult.Error;
        }

        Name = workspaceNameResult.Value;
        ModifiedOn = DateTime.UtcNow;
        
        return Result.Success();
    }
    
    public static Result<Workspace> Create(string name) =>
        Create(Guid.NewGuid(), DateTime.UtcNow, null, name);

    public static Result<Workspace> Create(Guid id, DateTime createdOn, DateTime? modifiedOn, string name)
    {
        Result<WorkspaceName> nameResult = WorkspaceName.Create(name);
        return nameResult.IsSuccess 
            ? new Workspace(id, createdOn, modifiedOn, nameResult.Value)
            : nameResult.Error;
    }
}
