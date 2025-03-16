using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Workspace : AuditableEntity
{
    public WorkspaceName Name { get; }
    public WorkspaceMember[] Members { get; }

    private Workspace(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        WorkspaceName name, WorkspaceMember[] members) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Members = members;
    }

    public static Result<Workspace> CreateNew(string name, User owner)
    {
        var utcNow = DateTime.UtcNow;
        
        return WorkspaceMember.CreateNew(owner, true)
            .Map(x => CreateExisting(
                Guid.NewGuid(), utcNow, utcNow,
                name, [x]));
    }

    public static Result<Workspace> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        string name, WorkspaceMember[] members) =>
        WorkspaceName.Create(name)
            .Map(x => new Workspace(id, createdOn, modifiedOn, x, members));
}