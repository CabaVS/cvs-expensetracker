using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class User(Guid id, DateTime createdOn, DateTime? modifiedOn) : Entity(id, createdOn, modifiedOn)
{
    public User() : this(Guid.NewGuid(), DateTime.UtcNow, null)
    {
    }
}
