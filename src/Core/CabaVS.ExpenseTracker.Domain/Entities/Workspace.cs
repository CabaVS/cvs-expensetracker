using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Workspace : Entity
{
    public WorkspaceName Name { get; set; }

    private Workspace(Guid id, WorkspaceName name) : base(id)
    {
        Name = name;
    }

    public static Result<Workspace> Create(Guid id, string name)
    {
        var nameResult = WorkspaceName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new Workspace(id, nameResult.Value);
    }
}