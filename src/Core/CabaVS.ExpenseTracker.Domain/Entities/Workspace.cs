using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Workspace : Entity
{
    public WorkspaceName Name { get; private set; }

    private Workspace(Guid id, WorkspaceName name) : base(id) => Name = name;

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
        
        return Result.Success();
    }

    public static Result<Workspace> Create(Guid id, string name)
    {
        Result<WorkspaceName> nameResult = WorkspaceName.Create(name);
        return nameResult.IsFailure ? nameResult.Error : new Workspace(id, nameResult.Value);
    }
}
