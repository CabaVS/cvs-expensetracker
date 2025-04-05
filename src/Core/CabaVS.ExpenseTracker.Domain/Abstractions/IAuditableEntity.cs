namespace CabaVS.ExpenseTracker.Domain.Abstractions;

public interface IAuditableEntity
{
    Guid Id { get; }
    DateTime CreatedOn { get; }
    DateTime ModifiedOn { get; }
}
