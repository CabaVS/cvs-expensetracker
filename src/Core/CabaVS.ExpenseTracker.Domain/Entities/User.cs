using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class User : AuditableEntity
{
    public UserName UserName { get; }
    public bool IsAdmin { get; }

    private User(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        UserName userName, bool isAdmin) : base(id, createdOn, modifiedOn)
    {
        UserName = userName;
        IsAdmin = isAdmin;
    }

    public static Result<User> CreateNew(string userName, bool isAdmin)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            userName, isAdmin);
    }

    public static Result<User> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        string userName, bool isAdmin) =>
        UserName.Create(userName)
            .Map(x => new User(id, createdOn, modifiedOn, x, isAdmin));
}
