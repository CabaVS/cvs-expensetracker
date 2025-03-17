using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class User : AuditableEntity
{
    public UserName UserName { get; }

    private User(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        UserName userName) : base(id, createdOn, modifiedOn) =>
        UserName = userName;

    public static Result<User> CreateNew(string userName)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            userName);
    }

    public static Result<User> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        string userName) =>
        UserName.Create(userName)
            .Map(x => new User(id, createdOn, modifiedOn, x));
}
