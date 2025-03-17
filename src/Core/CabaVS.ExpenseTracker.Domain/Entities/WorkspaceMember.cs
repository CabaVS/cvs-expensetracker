using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class WorkspaceMember : AuditableEntity
{
    public User User { get; }
    public bool IsAdmin { get; }

    private WorkspaceMember(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        User user, bool isAdmin) : base(id, createdOn, modifiedOn)
    {
        User = user;
        IsAdmin = isAdmin;
    }
    
    public static Result<WorkspaceMember> CreateNew(User user, bool isAdmin)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            user, isAdmin);
    }
    
    public static Result<WorkspaceMember> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        User user, bool isAdmin) =>
        Result<WorkspaceMember>.Success(
            new WorkspaceMember(id, createdOn, modifiedOn, user, isAdmin));
}
