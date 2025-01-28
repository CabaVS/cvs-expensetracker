using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class User(Guid id, DateTime createdOn, DateTime modifiedOn) : AuditableEntity(id, createdOn, modifiedOn);
