using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Workspace : Entity
{
    public WorkspaceName Name { get; }
    public bool IsAdmin { get; }

    private Workspace(Guid id, WorkspaceName name, bool isAdmin) : base(id)
    {
        Name = name;
        IsAdmin = isAdmin;
    }

    public static Result<Workspace> Create(Guid id, string name, bool isAdmin)
    {
        var nameResult = WorkspaceName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new Workspace(id, nameResult.Value, isAdmin);
    }
}